using Microsoft.Extensions.Configuration;
using PublishLinuxNGINX;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;


//string linuxProjectFolder=string.Empty;
//string tarFilePath= string.Empty;
//string tempFolder= string.Empty;
//string tempProjectFolder=string.Empty;
//string linuxWebFolder=string.Empty;
//string? serviceName = null;
//string? contentSubfolder= null;
//bool isServiceEnable = false;


string currentDir = Directory.GetCurrentDirectory();
string projPath = string.Empty;
switch (args.Length)
{
    case 0:
        var files = Directory.GetFiles(currentDir, "*.csproj");
        if (files.Length == 0)
            throw new Exception("The command line args must pass full path project file *.csproj or current directory must contains *.csproj file");
        if (files.Length > 1)
            throw new Exception("The Current directory must contains only one *.csproj file");
        projPath = files[0];
        break;
    case 1:
        projPath = args[0];
        break;
    default: throw new Exception("The command line args must pass full path project file *.csproj");
}
NGINXPublish config = new NGINXPublish(projPath);


config.BindWithAppSettings();
    Build();
    var linux = new Linux(config.TempFolder, config.BitviseTlpFile);
    putTolinux();
    bool serviceExist = stopService();
    MoveProjectFiles();
    configureLinuxService();
    if (serviceExist) startService();
    configureNGINX();



bool RunScript(string script,string? workingDirectory=null)
    {
        if (string.IsNullOrWhiteSpace(workingDirectory))
            workingDirectory = config.TempFolder;
        var proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/C {script}";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.WorkingDirectory = workingDirectory;
        proc.OutputDataReceived += (a, b) => Console.WriteLine(b.Data);
        proc.ErrorDataReceived += (a, b) => Console.WriteLine(b.Data);
        proc.StartInfo.CreateNoWindow = true;
        proc.Start();
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();
        proc.WaitForExit();
        return proc.ExitCode == 0 || proc.ExitCode==1000;
    }
bool sexec(string script) => RunScript($"sexec -profile=\"{config.BitviseTlpFile}\" -cmd=\"{script}\"");
bool sftpc(string script) => RunScript($"sftpc -profile=\"{config.BitviseTlpFile}\" -cmd=\"{script}\"");
void Build()
{
    if (Directory.Exists(config.PublishProjectFolder))
    {
        Directory.Delete(config.PublishProjectFolder, true);
    }
    Directory.CreateDirectory(config.PublishProjectFolder);

    if (!RunScript($"dotnet publish {config.ProjectFolder} --configuration Release  -o {config.PublishProjectFolder}"))
        throw new Exception("Error build project!");
    if (config.Service != null)
        File.WriteAllLines($"{config.PublishProjectFolder}/{config.ProjectName}.service", config.CreateServiceFileText());
    File.WriteAllLines($"{config.PublishProjectFolder}/{config.HostName}", config.GetNGINXConfig());
    File.Copy(config.SSLPrivate, $"{config.PublishProjectFolder}\\{config.HostName}.key");
    File.Copy(config.SSLPublic, $"{config.PublishProjectFolder}\\{config.HostName}.crt");
}
void putTolinux()
{
    if (File.Exists(config.TarFilePath))
        File.Delete(config.TarFilePath);
    // pack project
    if (!RunScript($"tar -czvf {config.TarFileName} {config.ProjectName}"))
        throw new Exception("Error pack !");
    // create temp linux folder
    if (!linux.DirectoryExist(config.LinuxTempFolder))
    {
        if (!linux.Mkdir(config.LinuxTempFolder))
           throw new Exception($"Error create folder {config.LinuxTempFolder} !");
    }
    // remove old folder  
    if (linux.DirectoryExist(config.LinuxTempProjectFolder))
    {
        if (!linux.RemoveFolder(config.LinuxTempProjectFolder))
            throw new Exception($"Error delete {config.LinuxTempProjectFolder}");
    }
 
    if (!sftpc($"cd {config.LinuxTempFolder};pwd;lcd {config.TempFolder};lpwd;put {config.TarFileName} -o;exit;"))
        throw new Exception("Error put file !");

    if (!sexec($"cd {config.LinuxTempFolder}; tar -xzvf {config.TarFileName}"))
        throw new Exception("Error unpack file!");
    Directory.Delete($"{config.TempFolder}/{config.ProjectName}", true);
}
bool stopService()
{
    if (config.Service == null) return false;
    Console.WriteLine($"Stop linux service {config.Service.ServiceName}");
    return sexec($"sudo service {config.Service.ServiceName} stop");
}
void startService()
{
    if (config.Service == null) return;
    Console.WriteLine($"Start linux service {config.Service.ServiceName}");
    sexec($"sudo service {config.Service.ServiceName} start");
}

void MoveProjectFiles()
{
    // check web folder
    if (!linux.DirectoryExist(config.PublishLinuxProjectFolder))
    {
        if (!linux.SudoMkdir(config.PublishLinuxProjectFolder))
            throw new Exception($"Error create folder {config.PublishLinuxProjectFolder}");
    }

    // clean webfolder
    if (!sexec($"sudo rm {config.PublishLinuxProjectFolder}/* -r"))
        Console.WriteLine($"Error clean {config.PublishLinuxProjectFolder}");

    // move content
    string sf = string.IsNullOrWhiteSpace(config.BlazorContent) ? "" : $"/{config.BlazorContent}";
    if (!sexec($"sudo mv {config.LinuxTempProjectFolder}{sf}/* {config.PublishLinuxProjectFolder}"))
        throw new Exception($"Error move content !");

    // set credential
    if (!sexec($"sudo chown www-data:www-data {config.PublishLinuxProjectFolder} -R"))
    {
        throw new Exception($"Error set www-data:www-data !");
    }
}

void configureLinuxService()
{
    if (config.Service == null) return;
    if (!sexec($"sudo mv {config.PublishLinuxProjectFolder}/{config.ProjectName}.service /etc/systemd/system/"))
        throw new Exception("Error move service file");
    if (!sexec($"sudo systemctl enable {config.ProjectName}"))
        throw new Exception("Error registered service");
    if (!sexec($"sudo systemctl daemon-reload"))
        throw new Exception("Error reload daemon service");
}

void configureNGINX()
{
    if (linux.FileExist($"/etc/nginx/sites-available/{config.HostName}"))
    {
        if (!sexec($"sudo cp /etc/nginx/sites-available/{config.HostName} /etc/nginx/sites-available/{config.HostName}.back"))
            throw new Exception("Error copy reserve copy");
    }
    if (!sexec($"sudo mv {config.PublishLinuxProjectFolder}/{config.HostName} /etc/nginx/sites-available/{config.HostName}"))
            throw new Exception("Error copy reserve copy");
    if (!linux.FileExist($"/etc/nginx/sites-enabled/{config.HostName}"))
    {
        if (!sexec($"sudo ln -s /etc/nginx/sites-available/{config.HostName} /etc/nginx/sites-enabled/{config.HostName}"))
            throw new Exception("Error copy reserve copy");
    }
    if (!sexec($"sudo mv {config.PublishLinuxProjectFolder}/{config.HostName}.crt /etc/ssl/certs/"))
        throw new Exception("Error copy reserve copy");
    if (!sexec($"sudo mv {config.PublishLinuxProjectFolder}/{config.HostName}.key /etc/ssl/private/"))
        throw new Exception("Error copy reserve copy");

    if (sexec($"sudo nginx -t"))
    {
        if (!sexec($"sudo nginx -s reload"))
            Console.WriteLine("Error start nginx");
    }
}

//void setToNGINX()
//    {
//        stopLinuxService();

//        // check web folder
//        if (!sexec($"if test -d '{linuxWebFolder}'; then echo 'The {linuxWebFolder} folder exists'; else sudo mkdir '{linuxWebFolder}'; fi"))
//            throw new Exception($"Error create {linuxWebFolder} !");
//        // clean webfolder
//        if (!sexec($"sudo rm {linuxWebFolder}/* -r"))
//            Console.WriteLine($"Error clean {linuxWebFolder}");

//        // move content
//        string sf = contentSubfolder == null ? "" : $"/{contentSubfolder}";
//        if (!sexec($"sudo mv {linuxProjectFolder}{sf}/* {linuxWebFolder}"))
//            throw new Exception($"Error move content !");

//        // set credential
//        if (!sexec($"sudo chown www-data:www-data {linuxWebFolder} -R"))
//        {
//            throw new Exception($"Error set www-data:www-data !");
//        }

//        configureNGINX();


//        startLinuxService();

//        if (!sexec($"sudo nginx -s reload"))
//            throw new Exception($"Error restart NGINX");

//    }

