using System.Diagnostics;
using System.Text.Json;

namespace CommonLibrary;

public class ProjectConfig
{
    public string ProjectFile { get; private set; }
    public string ProjectFolder { get; private set; }
    public string Version { get; private set; }
    public bool IsPreRelease { get; private set; }
    public  string OutputPath { get; private set; }
    public  string BuildConfiguration { get; private set; }

    public readonly System.Text.Json.JsonDocument jsonDocument = System.Text.Json.JsonDocument.Parse("{}");

    public ProjectConfig(string? projectFilePath)
	{
		if (projectFilePath == null)
			throw new ArgumentNullException();
		ProjectFile = projectFilePath;
        if (!File.Exists(ProjectFile))
            throw new Exception($"The file {ProjectFile} not exist!");
        // check extensions
        if (Path.GetExtension(ProjectFile) != ".csproj")
            throw new Exception($"The extensions file must be *.csproj!");
        
        var directory = Path.GetDirectoryName(ProjectFile);
        if (directory == null)
            throw new Exception($"The full project path is wrong:  {ProjectFile}");
        ProjectFolder = directory;
        
        // read data from xml
        var xml = new System.Xml.XmlDocument();
        xml.Load(ProjectFile);
        var version = xml.SelectSingleNode("Project/PropertyGroup/Version")?.InnerText;
        if (version == null)
            throw new Exception($"The project file {ProjectFile} do not include section <PropertyGroup/Version>");
        
        Version = version;
        var ver = new Version(version);
        IsPreRelease= ver.IsPreRelease;
        BuildConfiguration = IsPreRelease ?"Debug" : "Release";
        OutputPath = ProjectFolder + "\\bin\\" + BuildConfiguration;
        var configFile = $"{ProjectFolder}/SabatexSettings.json";
        if (File.Exists(configFile))
        {
            jsonDocument = System.Text.Json.JsonDocument.Parse(File.ReadAllText(configFile));
        }
    }

    public T? GetValue<T>(string key)
    {
        if (jsonDocument.RootElement.TryGetProperty(key,out var item))
        {
            return item.Deserialize<T>();
        }
        return default(T);
    }


    public bool RunScript(string script)
    {
        var proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/C {script}";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.WorkingDirectory = ProjectFolder;
        proc.OutputDataReceived += (a, b) => Console.WriteLine(b.Data);
        proc.ErrorDataReceived += (a, b) => Console.WriteLine(b.Data);
        proc.StartInfo.CreateNoWindow = true;
        proc.Start();
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();
        proc.WaitForExit();
        return proc.ExitCode == 0;
    }

}

