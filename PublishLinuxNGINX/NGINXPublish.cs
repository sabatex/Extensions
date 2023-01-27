using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishLinuxNGINX
{
    internal class NGINXPublish
    {
        /// <summary>
        /// The Bitvise tlp file path
        /// </summary>
        public string? BitviseTlpFile { get; set; }
        /// <summary>
        /// the temp local folder for build
        /// </summary>
        public string? TempFolder { get; set; }
        /// <summary>
        /// Temp folder in linux side - /home/azureuser/publish
        /// </summary>
        public string? LinuxTempFolder { get; set; }
        public string? LinuxWebFolder { get;set; }
        /// <summary>
        /// set service name or null if service not exist
        /// </summary>
        public string? ServiceName { get; set; }
        public string? ContentSubfolder { get; set; }
    }
}
