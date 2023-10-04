using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class Config
    {
        /// <summary>
        /// the temp folder for build 
        /// </summary>
        public string? TempFolder { get; set; }
 
        public Service? Service { get; set; }

        public readonly string ProjectFolder;
        public string PublishProjectFolder { get; private set; }
        public string PublishLinuxProjectFolder { get; private set; }
        public readonly string ProjectFilePath;
        public readonly string ProjectName;
 
       
 



    }


}
