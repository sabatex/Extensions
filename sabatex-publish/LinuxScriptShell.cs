using Sabatex.Publish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sabatex_publish;

public class LinuxScriptShell:ScriptShell
{
    readonly string bitviseTlpFile;
    public LinuxScriptShell(string workingDirectory,string? bitviseTlpFile) :base(workingDirectory)
    {
        if (string.IsNullOrWhiteSpace(bitviseTlpFile))
            throw new NullReferenceException(nameof(bitviseTlpFile));

        this.bitviseTlpFile = bitviseTlpFile;
    }

    public bool sexec(string script)
    {
        if (string.IsNullOrWhiteSpace(script))
            throw new ArgumentNullException(nameof(script));
         return Run($"sexec -profile=\"{bitviseTlpFile}\" -cmd=\"{script}\"");
    }
    public async Task<bool> sexecAsync(string script)
    {
        if (string.IsNullOrWhiteSpace(script))
            throw new ArgumentNullException(nameof(script));
        return await RunAsync($"sexec -profile=\"{bitviseTlpFile}\" -cmd=\"{script}\"");
    }

    bool sftpc(string script)
    {
        if (string.IsNullOrWhiteSpace(script))
            throw new ArgumentNullException(nameof(script));
        return Run($"sftpc -profile=\"{bitviseTlpFile}\" -cmd=\"{script}\"");
    }


    string SudoText(bool sudo) => sudo ? "sudo " : string.Empty;

    public bool FileExist(string? path)
    {
        return sexec($"test -f {path}");
    }
    public async Task<bool> FileExistAsync(string? path)
    {
        return await sexecAsync($"test -f {path}");
    }
    public bool DirectoryExist(string? path) => sexec($"test -d '{path}'");
    public bool Mkdir(string? path,bool sudoes = false) => sexec($"{SudoText(sudoes)}mkdir {path}");
    public bool RemoveFolder(string? path,bool sudoes = false) => sexec($"{SudoText(sudoes)}rm {path} -r");
    public async Task<bool> RemoveFolderAsync(string? path, bool sudoes = false) => await sexecAsync($"{SudoText(sudoes)}rm {path} -r");
    public async Task<bool> RemoveFileAsync(string? path, bool sudoes =false) => await sexecAsync($"{SudoText(sudoes)}rm {path}");

    public bool Move(string source, string destination,bool sudoes=false) => sexec($"{SudoText(sudoes)}mv {source} {destination}");

    public bool Copy(string source, string destination, bool sudoes = false) => sexec($"{SudoText(sudoes)}cp {source} {destination}");

    public bool Chown(string user, string path, bool sudoes = false) => sexec($"{SudoText(sudoes)}chown {user}:{user} {path} -R");

    public void PutFile(string localPath, string remotePath, string fileName)
    {
        if (!sftpc($"cd {remotePath};pwd;lcd {localPath};lpwd;put {fileName} -o;exit;"))
            throw new Exception("Error put file !");
    }

    public void PutFolder(string localPath, string remotePath, string folderName)
    {
        if (!sftpc($"cd {remotePath};pwd;lcd {localPath};lpwd;put {folderName} -o;exit;"))
            throw new Exception("Error put folder !");
    }

    public void UnPack(string linuxTempFolder,string tarFileName)
    {
        if (!sexec($"cd {linuxTempFolder}; tar -xzvf {tarFileName}"))
            throw new Exception("Error unpack file!");
    }

    public bool Tar(string tarFileName, string folderName,bool sudoes=false) => sexec($"{SudoText(sudoes)}tar -czvf {tarFileName} {folderName}");
    public bool EnableService(string serviceName) => sexec($"sudo systemctl enable {serviceName}");
    public bool DisableService(string serviceName) => sexec($"sudo systemctl disable {serviceName}");

    public bool StartService(string serviceName) => sexec($"sudo systemctl start {serviceName}");
    public bool StopService(string serviceName) => sexec($"sudo systemctl stop {serviceName}");
    public async Task<bool> StartServiceAsync(string serviceName) => await sexecAsync($"sudo systemctl start {serviceName}");
    public async Task<bool> StopServiceAsync(string serviceName) => await sexecAsync($"sudo systemctl stop {serviceName}");

    public bool RestartService(string serviceName) => sexec($"sudo systemctl restart {serviceName}");
    public bool StatusService(string serviceName) => sexec($"sudo systemctl status {serviceName}");
    public bool DaemonReload() => sexec($"sudo systemctl daemon-reload");
    public bool CreateSymlink(string source, string destination,bool sudoes=false) => sexec($"{SudoText(sudoes)}ln -s {source} {destination}");
    public bool RemoveSymlink(string source) => sexec($"rm {source}");
    public bool NginxTestConfig() => sexec($"sudo nginx -t");
    public bool NginxReload() => sexec($"sudo nginx -s reload");

    public bool DotnetRun(string path,string programm,string args,bool sudoes=false) => sexec($"cd {path};{SudoText(sudoes)}dotnet {programm} {args}");

}
