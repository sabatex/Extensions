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
        public readonly string ProjectName;
        public readonly string TempFolder;
        public readonly string ProjectFile;
        public readonly string ProjectFolder;
        public readonly string PublishProjectFolder;

        public Linux Linux { get; set; }
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
            Linux = new Linux(ProjectName);
            var conf = new ConfigurationBuilder().SetBasePath(ProjectFolder).AddJsonFile(configFileName).Build();
            conf.Bind(this);
            PublishProjectFolder = $"{TempFolder}\\{ProjectName}";
            Linux.Configure();
 


        }
    }
}
