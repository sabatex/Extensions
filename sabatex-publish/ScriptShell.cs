using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Publish;

public class ScriptShell
{
    protected readonly SabatexSettings Settings;
    private readonly string workingDirectory;

	public ScriptShell(SabatexSettings settings)
    {
        this.Settings = settings;
        workingDirectory = settings.TempFolder;
    }
    public bool Run(string script, string? workingDirectory = null)
    {
        var proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/C {script}";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;
        if (string.IsNullOrWhiteSpace(workingDirectory))
        {
            proc.StartInfo.WorkingDirectory = this.workingDirectory;
        }
        else
        {
            proc.StartInfo.WorkingDirectory = workingDirectory;
        }
        proc.OutputDataReceived += (a, b) => Console.WriteLine(b.Data);
        proc.ErrorDataReceived += (a, b) => Console.WriteLine(b.Data);
        proc.StartInfo.CreateNoWindow = true;
        proc.Start();
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();
        proc.WaitForExit(); 
        return proc.ExitCode == 0 || proc.ExitCode == 1000;
    }
	public async Task<bool> RunAsync(string script, string? workingDirectory = null)
	{
		var proc = new Process();
		proc.StartInfo.FileName = "cmd.exe";
		proc.StartInfo.Arguments = $"/C {script}";
		proc.StartInfo.UseShellExecute = false;
		proc.StartInfo.RedirectStandardOutput = true;
		proc.StartInfo.RedirectStandardError = true;
		if (string.IsNullOrWhiteSpace(workingDirectory))
		{
			proc.StartInfo.WorkingDirectory = this.workingDirectory;
		}
		else
		{
			proc.StartInfo.WorkingDirectory = workingDirectory;
		}
		proc.OutputDataReceived += (a, b) => Console.WriteLine(b.Data);
		proc.ErrorDataReceived += (a, b) => Console.WriteLine(b.Data);
		proc.StartInfo.CreateNoWindow = true;
		proc.Start();
		proc.BeginOutputReadLine();
		proc.BeginErrorReadLine();
		await proc.WaitForExitAsync();
		return proc.ExitCode == 0 || proc.ExitCode == 1000;
	}


	bool sexec(string script)
    {
        if (string.IsNullOrWhiteSpace(script))
            throw new ArgumentNullException(nameof(script));
        if (string.IsNullOrWhiteSpace(Settings.Linux.BitviseTlpFile))
            throw new NullReferenceException(nameof(Settings.Linux.BitviseTlpFile));
        return Run($"sexec -profile=\"{Settings.Linux.BitviseTlpFile}\" -cmd=\"{script}\"");
    }

    bool sftpc(string script)
    {
        if (string.IsNullOrWhiteSpace(script))
            throw new ArgumentNullException(nameof(script));
        if (string.IsNullOrWhiteSpace(Settings.Linux.BitviseTlpFile))
            throw new NullReferenceException(nameof(Settings.Linux.BitviseTlpFile));
        return Run($"sftpc -profile=\"{Settings.Linux.BitviseTlpFile}\" -cmd=\"{script}\"");
    }


    public bool DirectoryExist(string? path) => sexec($"test -d '{path}'");
    public bool Mkdir(string? path) => sexec($"mkdir {path}");
    public bool SudoMkdir(string? path) => sexec($"sudo mkdir {path}");
    public bool RemoveFolder(string? path) => sexec($"rm {path} -r");
    public bool FileExist(string? path)
    {
        return sexec($"test -f {path}");
    }

    public void Build()
    {
        if (Directory.Exists(Settings.TempPublishProjectFolder))
        {
            Directory.Delete(Settings.TempPublishProjectFolder, true);
        }
        Directory.CreateDirectory(Settings.TempPublishProjectFolder);
        string script = $"dotnet publish {Settings.ProjectFolder} --configuration Release  -o {Settings.TempPublishProjectFolder}";
        if (!Run(script, Settings.ProjectFolder))
            throw new Exception("Error build project!");
    }


    public void PutTolinux()
    {
        
        var tarFileName = Settings.Linux.TarFileName;
        var tarFilePath = $"{Settings.TempFolder}/{tarFileName}";
        var projectName = Settings.ProjectName;
        var tempFolder = Settings.TempFolder;
        if (string.IsNullOrWhiteSpace(Settings.Linux.TempFolder))
            throw new NullReferenceException(nameof(Settings.Linux.TempFolder));
        var linuxTempFolder = Settings.Linux.TempFolder;

        if (File.Exists(tarFilePath))
            File.Delete(tarFilePath);
        // pack project
        if (!Run($"tar -czvf {tarFileName} {projectName}", tempFolder))
            throw new Exception("Error pack !");
        // create temp linux folder
        if (!DirectoryExist(linuxTempFolder))
        {
            if (!Mkdir(linuxTempFolder))
                throw new Exception($"Error create folder {linuxTempFolder} !");
        }
        // remove old folder  
        if (DirectoryExist(Settings.Linux.TempProjectFolder))
        {
            if (!RemoveFolder(Settings.Linux.TempProjectFolder))
                throw new Exception($"Error delete {Settings.Linux.TempProjectFolder}");
        }

        if (!sftpc($"cd {linuxTempFolder};pwd;lcd {tempFolder};lpwd;put {tarFileName} -o;exit;"))
            throw new Exception("Error put file !");

        if (!sexec($"cd {linuxTempFolder}; tar -xzvf {tarFileName}"))
            throw new Exception("Error unpack file!");
        Directory.Delete($"{tempFolder}/{projectName}", true);
    }

    public void UpdateBackend()
    {
        var serviceName = Settings.Linux.Service.ServiceName;
        var publishFolder = Settings.Linux.PublishFolder;
        if (string.IsNullOrWhiteSpace(Settings.Linux.TempProjectFolder))
            throw new NullReferenceException(nameof(Settings.Linux.TempProjectFolder));
        var tempProjectFolder = Settings.Linux.TempProjectFolder;
        if (Settings.Linux.FrontEnd)
            throw new Exception("Try update backend for frontend project");
        Console.WriteLine($"Stop linux service {serviceName}");
        if (!sexec($"sudo service {serviceName} stop"))
            throw new Exception($"Do not stop service {serviceName}");

        // update service file

        // move project
        // check web folder
        if (!DirectoryExist(publishFolder))
        {
            //
            if (!SudoMkdir(publishFolder))
                throw new Exception($"Error create folder {publishFolder}");
        }
        else
        {
                // clean webfolder
                if (!sexec($"sudo rm {publishFolder}/* -r"))
                    Console.WriteLine($"Error clean {publishFolder}");
        }

        // move content
         if (!sexec($"sudo mv {tempProjectFolder}/* {publishFolder}"))
            throw new Exception($"Error move content !");
        // set credential
        if (!sexec($"sudo chown www-data:www-data {publishFolder} -R"))
        {
            throw new Exception($"Error set www-data:www-data !");
        }

        Console.WriteLine($"Start linux service {serviceName}");
        if (!sexec($"sudo service {serviceName} start"))
            throw new Exception($"Do not stop service {serviceName}");
    }

    public void UpdateBlazorwasm()
    {
        var publishFolder = Settings.Linux.PublishFolder;
        if (string.IsNullOrWhiteSpace(Settings.Linux.TempProjectFolder))
            throw new NullReferenceException(nameof(Settings.Linux.TempProjectFolder));
        var tempProjectFolder = Settings.Linux.TempProjectFolder;
         // check web folder
        if (!DirectoryExist(publishFolder))
        {
            //
            if (!SudoMkdir(publishFolder))
                throw new Exception($"Error create folder {publishFolder}");
        }
        else
        {
            // clean webfolder
            if (!sexec($"sudo rm {publishFolder}/* -r"))
                Console.WriteLine($"Error clean {publishFolder}");
        }

        // move content
        if (!sexec($"sudo mv {tempProjectFolder}/wwwroot/* {publishFolder}"))
            throw new Exception($"Error move content !");
        // set credential
        if (!sexec($"sudo chown www-data:www-data {publishFolder} -R"))
        {
            throw new Exception($"Error set www-data:www-data !");
        }

     }


}
