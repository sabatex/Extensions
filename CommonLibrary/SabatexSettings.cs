using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class SabatexSettings
    {
        const string configFileName = "SabatexSettings.json";
        public string ProjectName { get; private set; }
        public string TempFolder { get; private set; }
        public string ProjectFile { get; private set; }
        public string ProjectFolder { get; private set; }
        public string PublishProjectFolder { get; private set; }
        public string Version { get; private set; }
        public bool IsPreRelease { get; private set; }
        public string OutputPath { get; private set; }
        public string BuildConfiguration { get; private set; }


        public Linux Linux { get; set; }

        public NUGET NUGET { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectFile"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public SabatexSettings(string projectFile)
        {
           
            if (!File.Exists(projectFile))
            {
                throw new FileNotFoundException("The file not exist: ", projectFile);
            }

            if (Path.GetExtension(projectFile) != ".csproj")
                throw new Exception($"The file:{projectFile} must be extensions *.csproj!");
            ProjectFile = projectFile;
            
            ProjectFolder = Path.GetDirectoryName(projectFile) ?? string.Empty;
            if (ProjectFolder == string.Empty)
                throw new Exception($"Error get folder path from: {projectFile}");
            
            ProjectName = Path.GetFileNameWithoutExtension(projectFile);
            TempFolder = $"{Path.GetTempPath()}Sabatex";
            PublishProjectFolder = $"{TempFolder}\\{ProjectName}";
            // read data from xml
            var xml = new System.Xml.XmlDocument();
            xml.Load(projectFile);
            var version = xml.SelectSingleNode("Project/PropertyGroup/Version")?.InnerText;
            if (version == null)
                throw new Exception($"The project file {projectFile} do not include section <PropertyGroup/Version>");

            Version = version;
            var ver = new Version(version);
            IsPreRelease = ver.IsPreRelease;
            BuildConfiguration = IsPreRelease ? "Debug" : "Release";
            OutputPath = ProjectFolder + "\\bin\\" + BuildConfiguration;
            PublishProjectFolder = $"{TempFolder}\\{ProjectName}";

            Linux = new Linux(ProjectName);
            NUGET = new NUGET();
            var conf = new ConfigurationBuilder().SetBasePath(ProjectFolder).AddJsonFile(configFileName).Build();
            conf.Bind(this);
            Linux.Configure();
 
        }

        public static SabatexSettings GetConfig(string[] args)
        {
            switch (args.Length)
            {
                // no arguments
                case 0:

                    var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj");
                    if (files.Length == 0)
                        throw new Exception("The Current directory must contains  *.csproj file");
                    if (files.Length > 1)
                        throw new Exception("The Current directory must contains only one *.csproj file");
                    return new SabatexSettings(files[0]);
                case 1:
                    return new SabatexSettings(args[0]);
                default: throw new Exception("The command line args must pass full path project file *.csproj");
            }

        }


    }
}
