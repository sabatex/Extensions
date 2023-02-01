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
        [Value(0, MetaName = "csproj file", HelpText = "Path csproj file to be publish.", Required = true)]
        public string FileName { get; set; } = default!;
    }
}
