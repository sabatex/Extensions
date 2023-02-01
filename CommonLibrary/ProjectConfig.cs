using System.Diagnostics;

namespace CommonLibrary;

public class ProjectConfig
{
    public readonly string ProjectFilePath;
    public readonly string ProjectFolderPath;
    public readonly string Version;
    public readonly bool IsPreRelease;
    public readonly string OutputPath;
    public readonly string BuildConfiguration;
    
    public readonly string? NugetKeyPath;


	public ProjectConfig(string? projectFilePath)
	{
		if (projectFilePath == null)
			throw new ArgumentNullException();
		ProjectFilePath = projectFilePath;
        if (!File.Exists(ProjectFilePath))
            throw new Exception($"The file {ProjectFilePath} not exist!");
        // check extensions
        if (Path.GetExtension(ProjectFilePath) != ".csproj")
            throw new Exception($"The extensions file must be *.csproj!");
        var directory = Path.GetDirectoryName(ProjectFilePath);
        if (directory == null)
            throw new Exception($"The full project path is wrong:  {ProjectFilePath}");
        ProjectFolderPath = directory;
        // read data from xml
        var xml = new System.Xml.XmlDocument();
        xml.Load(ProjectFilePath);
        var version = xml.SelectSingleNode("Project/PropertyGroup/Version")?.InnerText;
        if (version == null)
            throw new Exception($"The project file {ProjectFilePath} do not include section <PropertyGroup/Version>");
        Version = version;
        var ver = new Version(version);
        IsPreRelease= ver.IsPreRelease;
        BuildConfiguration = IsPreRelease ?"Debug" : "Release";
        OutputPath = ProjectFolderPath + "\\bin\\" + BuildConfiguration;


        NugetKeyPath = xml.SelectSingleNode("Project/PropertyGroup/SabatexNugetKeyPath")?.InnerText;
    }
    public bool RunScript(string script)
    {
        var proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/C {script}";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.WorkingDirectory = ProjectFolderPath;
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

