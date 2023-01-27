using CommandLine;
using publishNuget;
using System.Diagnostics;


namespace PublishLinuxNGINX;

internal class Program
{
    static string projectFilePath = string.Empty;
    static string projectFolder = string.Empty;
    static string projectName = string.Empty;
    static string tarFileName= string.Empty;
    static string bitviseTlpFile= string.Empty;
    static string linuxTempFolder= string.Empty;
    static string linuxProjectFolder=string.Empty;
    static string tarFilePath= string.Empty;
    static string tempFolder= string.Empty;
    static string tempProjectFolder=string.Empty;
    static string linuxWebFolder=string.Empty;
    static string? serviceName = null;
    static string? contentSubfolder= null;
    static void setFolderPath(ref string value, string? folderName)
    {
        if (folderName == null)
        {
            Console.WriteLine($"The value folderName is null");
            Environment.Exit(1);
        }

        if (!Directory.Exists(folderName))
        {
            Console.WriteLine($"The directory {folderName} is not exist!");
            Environment.Exit(1);
        }
        value = folderName;
    }

    static void Initialize(string[] args)
    {
        var cmd = Parser.Default.ParseArguments<Options>(args);
        if (cmd.Errors.Count() != 0)
        {
            foreach (var error in cmd.Errors)
                Console.WriteLine(error);
            throw new Exception();
        }
        projectFilePath = cmd.Value.FileName;
        // check file exist
        if (!File.Exists(projectFilePath))
            throw new Exception($"The file {projectFilePath} not exist!");

        // check extensions
        if (Path.GetExtension(projectFilePath) != ".csproj")
            throw new Exception($"The extensions file must be *.csproj!");
        setFolderPath(ref projectFolder, Path.GetDirectoryName(projectFilePath));
        projectName = Path.GetFileNameWithoutExtension(projectFilePath);
        
        // get config
        string filePath = $"{projectFolder}/NGINXPublish.json";
        if (File.Exists(filePath))
        {
            string s = File.ReadAllText(filePath);
            var result = System.Text.Json.JsonSerializer.Deserialize<NGINXPublish>(s);
            if (result == null)
                throw new Exception($"Do nol load NGINXPublish.json!");
            if (string.IsNullOrWhiteSpace(result.TempFolder))
                throw new Exception($"Set the field TempFolder in NGINXPublish.json!");
            if (string.IsNullOrWhiteSpace(result.BitviseTlpFile))
                throw new Exception($"Set the field BitviseTlpFile in NGINXPublish.json!");
            if (string.IsNullOrWhiteSpace(result.LinuxTempFolder))
                throw new Exception($"Set the field LinuxTempFolder in NGINXPublish.json!");
            if (string.IsNullOrWhiteSpace(result.LinuxWebFolder))
                throw new Exception($"Set the field LinuxWebFolder in NGINXPublish.json!");
            serviceName= result.ServiceName;
            contentSubfolder = result.ContentSubfolder;
            linuxWebFolder= result.LinuxWebFolder;
            bitviseTlpFile = result.BitviseTlpFile;
            linuxTempFolder = result.LinuxTempFolder;
            linuxProjectFolder = $"{linuxTempFolder}/{projectName}";
            tarFileName = $"{projectName}.tar.gz";
            tempFolder = result.TempFolder;
            tarFilePath = $"{tempFolder}/{tarFileName}";
            tempProjectFolder= $"{tempFolder}/{projectName}";

        }
        else
        {
            // crete config file
            string s = System.Text.Json.JsonSerializer.Serialize(new NGINXPublish());
            File.WriteAllText(filePath, s);
            throw new Exception($"Fill the NGINXPublish.json!");
        }

    }
    
    static bool RunScript(string script,string? workingDirectory=null)
    {
        if (string.IsNullOrWhiteSpace(workingDirectory))
            workingDirectory = tempFolder;
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
    static bool sexec(string script) => RunScript($"sexec -profile=\"{bitviseTlpFile}\" -cmd=\"{script}\"");
    static bool sftpc(string script) => RunScript($"sftpc -profile=\"{bitviseTlpFile}\" -cmd=\"{script}\"");

    static void Build()
    {
        if (Directory.Exists(tempProjectFolder))
        {
            Directory.Delete(tempProjectFolder, true);
        }
        Directory.CreateDirectory(tempProjectFolder);

        if (!RunScript($"dotnet publish {projectFolder} --configuration Release  -o {tempProjectFolder}"))
            throw new Exception("Error build project!");
    }
    static void putTolinux()
    {
        if (File.Exists(tarFilePath))
            File.Delete(tarFilePath);

        if (!RunScript($"tar -czvf {tarFileName} {projectName}"))
           throw new Exception("Error pack !");

        if (!sexec($"if test -d '{linuxTempFolder}'; then echo 'The {linuxTempFolder} folder exists'; else mkdir '{linuxTempFolder}'; fi"))
            throw new Exception("Error pack !");

        if (!sexec($"if test -d '{linuxProjectFolder}'; then rm {linuxProjectFolder} -r; else echo; fi"))
            throw new Exception("Error pack !");

        if (!sftpc($"cd {linuxTempFolder};pwd;lcd {tempFolder};lpwd;put {tarFileName} -o;exit;"))
           throw new Exception("Error put file !");

        if (!sexec($"cd {linuxTempFolder}; tar -xzvf {tarFileName}"))
            throw new Exception("Error unpack file!");

    }

    static void setToNGINX()
    {
        if (serviceName!=null)
        {
            if (!sexec($"sudo service {serviceName} stop"))
                throw new Exception($"Error stop service {serviceName}");

        }
        // check web folder
        if (!sexec($"if test -d '{linuxWebFolder}'; then echo 'The {linuxWebFolder} folder exists'; else sudo mkdir '{linuxWebFolder}'; fi"))
            throw new Exception($"Error create {linuxWebFolder} !");
        // clean webfolder
        if (!sexec($"sudo rm {linuxWebFolder}/* -r"))
            Console.WriteLine($"Error clean {linuxWebFolder}");
        
        // move content
        string sf = contentSubfolder == null ? "" : $"/{contentSubfolder}";
        if (!sexec($"sudo mv {linuxProjectFolder}{sf}/* {linuxWebFolder}"))
            throw new Exception($"Error move content !");
       
        // set credential
        if (!sexec($"sudo chown www-data:www-data {linuxWebFolder} -R"))
        {
            throw new Exception($"Error set www-data:www-data !");
        }


        if (serviceName != null)
        {

            if (!sexec($"sudo service {serviceName} start"))
                throw new Exception($"Error start service {serviceName}");
        }

        if (!sexec($"sudo nginx -s reload"))
            throw new Exception($"Error restart NGINX");

    }

    static void Main(string[] args)
    {
        try
        {
            Initialize(args);
            Build();
            putTolinux();
            setToNGINX();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            Environment.Exit(1);
        }
    }
}