using CommandLine;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publishNuget
{
    internal class Options
    {
        [Option('t',"nuget-token",Required =false,HelpText = "nuget auth token")]
        public string? NugetAuthToken { get; set; }
        [Option('p', "nuget-token-path", Required = false, HelpText = "nuget auth token file path")]
        public string? NugetAuthTokenPath { get; set; }

        [Value(0, MetaName = "csproj file", HelpText = "Path csproj file to be publish.", Required = true)]
        public string FileName { get; set; } = default!;
    }
}
