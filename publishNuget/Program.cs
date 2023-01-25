// See https://aka.ms/new-console-template for more information
using CommandLine;
using publishNuget;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Data.SqlTypes;
using System.Reflection;
using System.Diagnostics;

namespace publishNuget;
internal class Program
{
    
    static string projectFilePath { get; set; }=string.Empty;
    static string? NugetAuthToken { get; set; }
    public static string? NugetAuthTokenPath { get; set; }
    static Nuget NugetConfig { get; set; } = new Nuget();


    static string nugetCongigFileName = string.Empty;
    static string projectFolder = string.Empty;
    static string outputFolder = string.Empty;

    static void ParseCMD(string[] args)
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
        NugetAuthToken = cmd.Value.NugetAuthToken;
        NugetAuthTokenPath= cmd.Value.NugetAuthTokenPath;

        //check nuget auth token
        if (NugetAuthToken == null)
        {
            string defaultTokenFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.nuget\\NUGET_AUTH_TOKEN";
            if (NugetAuthTokenPath != null)
                defaultTokenFile = NugetAuthTokenPath;
            //token pass as path to string file
            if (!File.Exists(defaultTokenFile))
                throw new Exception($"Do not exist file {defaultTokenFile}");

            var s = File.ReadAllLines(defaultTokenFile);
            if (s.Length != 1)
                throw new Exception("The file with token is wrong");
            NugetAuthToken = s[0];
        }
    }
    static Nuget getNugetConfigFile()
    {
        if (File.Exists(nugetCongigFileName))
        {
            string s = File.ReadAllText(nugetCongigFileName);
            var result = System.Text.Json.JsonSerializer.Deserialize<Nuget>(s);
            if (result == null)
                throw new Exception($"Do nol load nuget.json!");
            return result;
        }
        return new Nuget();
    }

    static void setFolderPath(ref string value,string? folderName)
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
    static string? getVersionFomProjectFile()
    {
        var xml = new System.Xml.XmlDocument();
        xml.Load(projectFilePath);
        return xml.SelectSingleNode("Project/PropertyGroup/Version")?.InnerText;
    }
    
    
    static void checkNugetConfig()
    {
        if (NugetConfig.ProjVersion != null)
        {
            if (NugetConfig.ProjVersion != NugetConfig.Version)
            {
                var verProj = new Version(NugetConfig.ProjVersion);
                var ver = new Version(NugetConfig.Version);
                NugetConfig.Version = NugetConfig.ProjVersion;
                if ((verProj.Major != ver.Major || verProj.Minor != ver.Minor) && ver.Point == 0)
                {
                    NugetConfig.Stage = BuildStages.Alpha;
                    NugetConfig.BuildVersion = 0;
                    return;
                }
                
                if (verProj.Point != 0)
                {
                    // set release
                    NugetConfig.Stage = BuildStages.Release;
                    NugetConfig.BuildVersion = 0;
                }
            }
        
        }

    }

    static bool RunScript(string script)
    {
        var proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/C {script}";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError= true;
        proc.StartInfo.WorkingDirectory = projectFolder;
        proc.OutputDataReceived += (a, b) => Console.WriteLine(b.Data);
        proc.ErrorDataReceived += (a, b) => Console.WriteLine(b.Data);
        proc.StartInfo.CreateNoWindow = true;
        proc.Start();
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();
        proc.WaitForExit();
        return proc.ExitCode == 0;
    }


    static void Main(string[] args)
    {
        try
        {
            ParseCMD(args);
            setFolderPath(ref projectFolder, Path.GetDirectoryName(projectFilePath));
            nugetCongigFileName = $"{projectFolder}/nuget.json";
            NugetConfig = getNugetConfigFile();
            NugetConfig.ProjVersion = getVersionFomProjectFile();
            checkNugetConfig();
            outputFolder = projectFolder + "\\bin\\" + NugetConfig.BuildConfiguration;
            RunScript($"del {outputFolder}\\*.nupkg");
            string includeSource = NugetConfig.Stage == BuildStages.Release ? string.Empty : "--include-source";
            string script = $"dotnet pack --configuration {NugetConfig.BuildConfiguration} {projectFilePath} -p:PackageVersion={NugetConfig} {includeSource}";
            if (!RunScript(script))
                throw new Exception("Error build project!");
 
            string symbols = NugetConfig.Stage == BuildStages.Release ? string.Empty : ".symbols";
            script = $"dotnet nuget push {outputFolder}\\*{symbols}.nupkg -k {NugetAuthToken} -s https://api.nuget.org/v3/index.json --skip-duplicate";
            if (!RunScript(script))
                throw new Exception("Error publish to nuget!");
            NugetConfig.BuildVersion++;

            string s = System.Text.Json.JsonSerializer.Serialize(NugetConfig);
            File.WriteAllText(nugetCongigFileName, s);
            Console.WriteLine("Done!");
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
        //var builder = new ConfigurationBuilder();
        //builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        //builder.AddUserSecrets<Program>();
        //var Configuration = builder.Build();

        
        
        #region
 
        #endregion

    }

}



