using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishLinuxNGINX
{
    internal static class ScriptShell
    {
        public static string WorkingDirectory = Environment.CurrentDirectory;
        public static bool Run(string script, string? workingDirectory = null)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = $"/C {script}";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                proc.StartInfo.WorkingDirectory = WorkingDirectory;
            }else
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

    }
}
