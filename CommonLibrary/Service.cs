using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class Service
    {
        /// <summary>
        /// Service name
        /// defaul set as ProjectName
        /// </summary>
        public string ServiceName { get; set; }
        [JsonIgnore]
        public string? ProjectFolder { get; set; }
        [JsonIgnore]
        public string? ProjectExecutedFileName { get; set; }

        public int Port { get; set; } = 5000;
        public bool UpdateService { get; set; } = false;
        public IEnumerable<string> GetConfig()
        {
            yield return "[Unit]";
            yield return $"Description = {projectName}";
            yield return "[Service]";
            yield return $"WorkingDirectory ={ProjectFolder}";
            yield return $"ExecStart =/usr/bin/dotnet {ProjectFolder}/{ProjectExecutedFileName}.dll";
            yield return "Restart=always";
            yield return "RestartSec=10";
            yield return "KillSignal=SIGINT";
            yield return $"SyslogIdentifier={projectName}";
            yield return "User=www-data";
            yield return "Environment=ASPNETCORE_ENVIRONMENT=Production";
            yield return "Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false";
            yield return $"Environment=ASPNETCORE_URLS=http://localhost:{Port}";
            yield return "[Install]";
            yield return "WantedBy=multi-user.target";
        }



        private readonly string projectName;
        public Service(string projectName)
        {
            this.projectName = projectName;
            ServiceName = projectName;
        }


    }
}
