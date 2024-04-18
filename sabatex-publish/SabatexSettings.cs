using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sabatex.Publish;

public class SabatexSettings
{
    const string configFileName = "appsettings.json";
	readonly string  _projectFilePath;
	readonly string _version;

	#region bindable from appsetings.json
	/// <summary>
	/// May be set from project appsetings.json sabatex:TempFolder,
	/// default {user temp folder}/sabatex
	/// </summary>
	public string TempFolder { get; set; }

	#endregion
	/// <summary>
	/// The project name set as csproj file name
	/// </summary>
	public string ProjectName => Path.GetFileNameWithoutExtension(_projectFilePath);
    public string ProjectFolder
	{
		get
		{
			var result = Path.GetDirectoryName(_projectFilePath) ?? string.Empty;
			if (result == string.Empty)
			{
				throw new Exception($"Error get folder path from: {_projectFilePath}");
			}
			return result;
		}
	}
	public string Version => _version;

	public string TempPublishProjectFolder { get; private set; } = default!;
	
    public bool IsPreRelease { get; private set; }
    public string OutputPath { get; private set; }
    public string BuildConfiguration { get; private set; }


    public Linux Linux { get; private set; }

    public NUGET NUGET { get; private set; }=new NUGET();


    public bool IsLibrary { get; private set; }
    public SabatexSettings(string projectFile, bool setup=false)
    {
		if (!File.Exists(projectFile))
		{
			throw new FileNotFoundException("The file not exist: ", projectFile);
		}

		if (Path.GetExtension(projectFile) != ".csproj")
		{
			throw new Exception($"The file:{projectFile} must be extensions *.csproj!");
		}

		_projectFilePath = projectFile;
		
		// read csproj data
		var xml = new System.Xml.XmlDocument();
		xml.Load(projectFile);
		var version = xml.SelectSingleNode("Project/PropertyGroup/Version")?.InnerText;
		if (version == null)
		{
			throw new Exception($"The project file {projectFile} do not include section <PropertyGroup/Version>");
		}

		_version = version;
		var ver = new Version(version);
		IsPreRelease = ver.IsPreRelease;
		BuildConfiguration = IsPreRelease ? "Debug" : "Release";
		OutputPath = ProjectFolder + "\\bin\\" + BuildConfiguration;
		
		var sdk = xml.SelectSingleNode("Project")?.Attributes?.GetNamedItem("Sdk")?.Value;
		if (sdk == null)
		{
			throw new Exception($"Do not read SDK type from project");
		}

		switch (sdk)
		{
			case "Microsoft.NET.Sdk.Web":
				break; //exe
			case "Microsoft.NET.Sdk":
			case "Microsoft.NET.Sdk.Razor":
                var library = xml.SelectSingleNode("Project/PropertyGroup/OutputType")?.InnerText;
				if (library == null || library.ToLower() != "exe")
					IsLibrary = true;
				break;
			default: throw new Exception($"Uknown SDK type");

		}

		var userSecretId = xml.SelectSingleNode("Project/PropertyGroup/UserSecretsId")?.InnerText;



        Linux = new Linux(ProjectName);
		var builder = new ConfigurationBuilder().SetBasePath(ProjectFolder);
		if (File.Exists(configFileName))
			builder.AddJsonFile(configFileName);
		if (!String.IsNullOrWhiteSpace(userSecretId))
			builder.AddUserSecrets(userSecretId);


        var conf = builder.Build();
		var sabatexSection = conf.GetSection("SabatexSettings");
		if (sabatexSection == null)
		{
			throw new Exception("The file appsetting.json d'nt contains section Sabatex!!! ");
		}
	    sabatexSection.Bind(this);
		
		if (TempFolder == null)
		{
			TempFolder = $"{Path.GetTempPath()}Sabatex";
		}

		TempPublishProjectFolder = $"{TempFolder}\\{ProjectName}";

	}

 
}
