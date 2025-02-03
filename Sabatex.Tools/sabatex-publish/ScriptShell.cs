using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Publish;

public class ScriptShell
{
    //protected readonly SabatexSettings Settings;
    private readonly string workingDirectory;

	public ScriptShell(string workingDirectory)
    {
        //this.Settings = settings;
        this.workingDirectory = workingDirectory;
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
        var processInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/C {script}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory ?? this.workingDirectory
        };



        using (var process = new Process { StartInfo = processInfo })
        {
            try
            {
                process.Start();
                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();
                await Task.WhenAll(outputTask, errorTask);
                Console.WriteLine(outputTask.Result);
                Console.WriteLine(errorTask.Result);
                return process.ExitCode == 0 || process.ExitCode == 1000;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
	}
}
