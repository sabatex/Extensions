// See https://aka.ms/new-console-template for more information
using CommandLine;
using publishNuget;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Data.SqlTypes;
using System.Reflection;
using System.Diagnostics;
using CommonLibrary;

namespace publishNuget;
internal class Program
{    
    static void Main(string[] args)
    {
        try
        {
            var cmd = Parser.Default.ParseArguments<Options>(args);
            if (cmd.Errors.Count() != 0)
            {
                foreach (var error in cmd.Errors)
                    Console.WriteLine(error);
                throw new Exception();
            }

            ProjectConfig projectConfig = new ProjectConfig(cmd.Value.FileName);
            if (projectConfig.NugetKeyPath == null)
                throw new Exception("The value <SabatexNugetKeyPath> must be defined in section <PropertyGroup>");

            string[] token = File.ReadAllLines(projectConfig.NugetKeyPath);
            if (token.Length == 0 || token.Length > 1)
                throw new Exception("The NUGET TOKEN is wrong!");
            string nugetAuthToken = token[0];
            
            projectConfig.RunScript($"del {projectConfig.OutputPath}\\*.nupkg");

            string includeSource =projectConfig.IsPreRelease ? "--include-source":string.Empty;
            string script = $"dotnet pack --configuration {projectConfig.BuildConfiguration} {projectConfig.ProjectFilePath} {includeSource}";
            if (!projectConfig.RunScript(script))
                throw new Exception("Error build project!");
 
            string symbols =  projectConfig.IsPreRelease? ".symbols": string.Empty;
            script = $"dotnet nuget push {projectConfig.OutputPath}\\*{symbols}.nupkg -k {nugetAuthToken} -s https://api.nuget.org/v3/index.json --skip-duplicate";
            if (!projectConfig.RunScript(script))
                throw new Exception("Error publish to nuget!");
            Console.WriteLine("Done!");
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
    }

}



