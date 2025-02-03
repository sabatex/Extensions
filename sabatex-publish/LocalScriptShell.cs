using Sabatex.Publish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sabatex_publish;

internal class LocalScriptShell:ScriptShell
{
    public LocalScriptShell(string workingDirectory) :base(workingDirectory)
    {
        
    }

    public bool Delete(string path) => Run($"del {path}");

}
