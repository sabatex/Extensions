using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        /// the temp folder for build 
        /// </summary>
        public string? TempFolder { get; set; }
        /// <summary>
        /// Temp folder in linux side - /home/azureuser/publish
        /// </summary>
        public string? LinuxTempFolder { get; set; }
        public string? LinuxWebFolder { get;set; }
        public string? BlazorContent { get; set; }

        public string? HostName { get; set; }
        public string? SSLPrivate { get;set; }
        public string? SSLPublic { get; set; }

        public Service? Service { get; set; }

        public readonly string ProjectFolder;
        public string PublishProjectFolder { get; private set; }
        public string PublishLinuxProjectFolder { get; private set; }
        public readonly string ProjectFilePath;
        public readonly string ProjectName;
        public readonly string TarFileName;
        public string TarFilePath { get; private set; }
        public string LinuxTempProjectFolder { get; private set; }



        public NGINXPublish(string projectFilePath)
        {
            ProjectFilePath = projectFilePath;
            if (!File.Exists(projectFilePath))
                throw new Exception($"The file {projectFilePath} not exist!");
             // check extensions
             if (Path.GetExtension(projectFilePath) != ".csproj")
                    throw new Exception($"The extensions file must be *.csproj!");
             ProjectFolder = Path.GetDirectoryName(projectFilePath) ?? string.Empty;
                if (ProjectFolder == string.Empty)
                throw new Exception("Error project folder path");
            ProjectName = Path.GetFileNameWithoutExtension(projectFilePath);
            TarFileName = $"{ProjectName}.tar.gz";
        }
        public void BindWithAppSettings()
        {
            var conf = new ConfigurationBuilder()
                       .SetBasePath(ProjectFolder)
                       .AddJsonFile("SabatexSettings.json")
                       .Build();
            conf.GetSection("NGINXPublish").Bind(this);

            if (string.IsNullOrWhiteSpace(TempFolder))
            {
                TempFolder =$"{Path.GetTempPath()}Sabatex";
            }
            if (string.IsNullOrWhiteSpace(LinuxTempFolder))
                throw new Exception($"Set the field LinuxTempFolder in NGINXPublish.json!");
            if (string.IsNullOrWhiteSpace(LinuxWebFolder))
                throw new Exception($"Set the field LinuxWebFolder in NGINXPublish.json!");
            if (Service != null)
            {
                if (string.IsNullOrWhiteSpace(Service.ServiceName))
                    Service.ServiceName = ProjectName;
            }
            PublishProjectFolder = $"{TempFolder}\\{ProjectName}";
            PublishLinuxProjectFolder = $"{LinuxWebFolder}/{ProjectName}";
            TarFilePath = $"{TempFolder}/{TarFileName}";
            LinuxTempProjectFolder = $"{LinuxTempFolder}/{ProjectName}";
            //linuxProjectFolder = 

            //tempFolder = config.TempFolder;
            //tarFilePath = 

            //isServiceEnable = checkExistConfigLinuxService();

        }




        public IEnumerable<string> GetNGINXConfig()
        {
            yield return "server {";
            yield return "    listen 80;";
            yield return $"    server_name {HostName};";
            yield return "    location / {";
            yield return "        add_header Strict-Transport-Security max-age=15768000;";
            yield return "        return 301 https://$host$request_uri;";
            yield return "    }";
            yield return "}";
            yield return "";
            yield return "server {";
            yield return "    listen *:443              ssl;";
            yield return $"    server_name               {HostName};";
            yield return $"    ssl_certificate           /etc/ssl/certs/{HostName}.crt;";
            yield return $"    ssl_certificate_key       /etc/ssl/private/{HostName}.key;";
            yield return "    ssl_protocols             TLSv1.1 TLSv1.2;";
            yield return "    ssl_prefer_server_ciphers on;";
            yield return "    ssl_ciphers               \"EECDH+AESGCM:EDH+AESGCM:AES256+EECDH:AES256+EDH\";";
            yield return "    ssl_ecdh_curve            secp384r1;";
            yield return "    ssl_session_cache         shared:SSL:10m;";
            yield return "    ssl_session_tickets       off;";
            yield return "    ssl_stapling              on;";
            yield return "    ssl_stapling_verify       on;";
            yield return "";
            yield return "    add_header Strict-Transport-Security \"max-age=63072000; includeSubdomains; preload\";";
            yield return "    add_header X-Frame-Options           DENY;";
            yield return "    add_header X-Content-Type-Options    nosniff;";
            yield return "    proxy_redirect   off;";
            yield return "    proxy_set_header Host             $host;";
            yield return "    proxy_set_header X-Real-IP        $remote_addr;";
            yield return "    proxy_set_header X-Forwarded-For  $proxy_add_x_forwarded_for;";
            yield return "    proxy_set_header X-Forwarded-Proto $scheme;";
            yield return "    client_max_body_size    10m;";
            yield return "    client_body_buffer_size 128k;";
            yield return "    proxy_connect_timeout   90;";
            yield return "    proxy_send_timeout      90;";
            yield return "    proxy_read_timeout      90;";
            yield return "    proxy_buffers        8 16k;";
            yield return "    proxy_buffer_size    16k;";
            yield return "";
            yield return "    #Redirects all traffic";
            yield return "    location / {";
            yield return $"        proxy_pass       http://localhost:{Service?.Port ?? 5000};";
            yield return "    }";
            yield return "}";
        }

        public IEnumerable<string> CreateServiceFileText()
        {
            yield return "[Unit]";
            yield return $"Description = {ProjectName}";
            yield return "[Service]";
            yield return $"WorkingDirectory ={PublishLinuxProjectFolder}";
            yield return $"ExecStart =/usr/bin/dotnet {PublishLinuxProjectFolder}/{ProjectName}.dll";
            yield return "Restart=always";
            yield return "RestartSec=10";
            yield return "KillSignal=SIGINT";
            yield return $"SyslogIdentifier={ProjectName}";
            yield return "User=www-data";
            yield return "Environment=ASPNETCORE_ENVIRONMENT=Production";
            yield return "Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false";
            yield return $"Environment=ASPNETCORE_URLS=http://localhost:{Service?.Port ?? 5000}";
            yield return "[Install]";
            yield return "WantedBy=multi-user.target";
        }

    }
}
