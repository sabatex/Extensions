using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class NGINX
    {
        public bool UpdateNGINX { get; set; } = false;
        public string? SSLPrivateName { get; set; }
        public string? SSLPrivateLocalPath { get; set; }
        public string? SSLPublicName { get; set; }
        public string? SSLPublicLocalPath { get; set; }
        public string[]? HostName { get; set; }
        private string? HostNames => string.Join(",", HostName ?? new string[] { });

        public int AppPort { get; set; } = 5000;

        public bool UpdateConfig { get; set; } 
        public IEnumerable<string> GetConfig()
        {
            yield return "server {";
            yield return "    listen 80;";
            yield return $"    server_name {HostNames};";
            yield return "    location / {";
            yield return "        add_header Strict-Transport-Security max-age=15768000;";
            yield return "        return 301 https://$host$request_uri;";
            yield return "    }";
            yield return "}";
            yield return "";
            yield return "server {";
            yield return "    listen *:443              ssl;";
            yield return $"    server_name               {HostNames};";
            yield return $"    ssl_certificate           /etc/ssl/certs/{SSLPublicName}.crt;";
            yield return $"    ssl_certificate_key       /etc/ssl/private/{SSLPrivateName}.key;";
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
            yield return $"        proxy_pass       http://localhost:{AppPort};";
            yield return "    }";
            yield return "}";
        }

    }
}
