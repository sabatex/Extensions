using CommonLibrary;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace PublishLinux
{
    internal class Program
    {
        static void PostBuild(SabatexSettings config)
        {
            File.WriteAllLines($"{config.PublishProjectFolder}/{config.ProjectName}.service", config.Linux.Service.GetConfig());
            File.WriteAllLines($"{config.PublishProjectFolder}/{config.ProjectName}", config.Linux.NGINX.GetConfig());
            if (config.Linux.NGINX.SSLPrivateLocalPath != null)
                File.Copy(config.Linux.NGINX.SSLPrivateLocalPath, $"{config.PublishProjectFolder}\\{config.Linux.NGINX.SSLPrivateName}.key");
            if (config.Linux.NGINX.SSLPublicLocalPath != null)
                File.Copy(config.Linux.NGINX.SSLPublicLocalPath, $"{config.PublishProjectFolder}\\{config.Linux.NGINX.SSLPublicName}.crt");

        }
        
        static SabatexSettings GetConfig(string[] args) 
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

        static void Main(string[] args)
        {
            var config = GetConfig(args);
            var ss = new ScriptShell(config);
            ss.Build();
            PostBuild(config);
            ss.PutTolinux();
            if (!config.Linux.FrontEnd)
                ss.UpdateBackend();
            else
                ss.UpdateBlazorwasm();
        }
    }
}