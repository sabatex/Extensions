using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishLinuxNGINX;

internal class Linux
{
    private readonly string workingDirectory;
    private readonly string BitviseTlpFile;
    public Linux(string? workingDirectory, string? bitviseTlpFile)
    {
        if (string.IsNullOrWhiteSpace(bitviseTlpFile))
            throw new Exception($"Set the field BitviseTlpFile!");
        if (string.IsNullOrWhiteSpace(workingDirectory))
            throw new Exception("The working directory is null");
        this.workingDirectory = workingDirectory;
        BitviseTlpFile = bitviseTlpFile;

    }

    bool RunScript(string script)
    {
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
        return proc.ExitCode == 0 || proc.ExitCode == 1000;
    }
    bool sexec(string script) => RunScript($"sexec -profile=\"{BitviseTlpFile}\" -cmd=\"{script}\"");
    bool sftpc(string script) => RunScript($"sftpc -profile=\"{BitviseTlpFile}\" -cmd=\"{script}\"");

    public bool DirectoryExist(string? path) => sexec($"test -d '{path}'");
    public bool Mkdir(string? path) => sexec($"mkdir {path}");
    public bool RemoveFolder(string? path) => sexec($"rm {path} -r");
    public bool FileExist(string? path)
    {
        return sexec($"test -f {path}");
    }
 
}
