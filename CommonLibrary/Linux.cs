using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class Linux
    {
        public Service Service { get; set; }
        public NGINX NGINX { get; set; }
        /// <summary>
        /// user home folder
        /// The publisher ignore Linux section if null value 
        /// </summary>
        public string? UserHomeFolder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool FrontEnd { get; set; }
        /// <summary>
        /// Publish folder on linux server
        /// default : /var/www/{ProjectName}
        /// </summary>
        public string PublishFolder { get; set; }

        /// <summary>
        /// Temp folder in linux side - /home/azureuser/publish
        /// </summary>
        public string? TempFolder => string.IsNullOrWhiteSpace(UserHomeFolder) ? null : UserHomeFolder + "/sabatex";
        /// <summary>
        /// Temp project Folder - /home/azureuser/publish/{projectName}
        /// </summary>
        public string? TempProjectFolder => string.IsNullOrWhiteSpace(TempFolder) ? null : TempFolder + $"/{projectName}";
        /// <summary>
        /// The Bitvise tlp file path
        /// </summary>
        public string? BitviseTlpFile { get; set; }
        
        public readonly string TarFileName;
        public string TarFilePath { get; private set; }
       
        public string PublishLinuxProjectFolder { get; private set; }
        readonly string  projectName;




        public Linux(string projectName) 
        {
            this.projectName = projectName;
            Service = new Service(projectName);
            NGINX = new NGINX();
            TarFileName = $"{projectName}.tar.gz";
            FrontEnd = false ;
            PublishFolder = "/var/www/" + projectName;
        }

        public void Configure()
        {
            if (UserHomeFolder == null) return;
        }

    }
}
