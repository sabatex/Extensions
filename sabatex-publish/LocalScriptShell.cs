using Sabatex.Publish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sabatex_publish;

internal class LocalScriptShell:ScriptShell
{
    public LocalScriptShell(SabatexSettings settings):base(settings)
    {
        
    }

    public bool Delete(string path) => Run($"del {path}");
    public async Task PackAsync()
    {
		string includeSource = Settings.IsPreRelease ? "--include-source" : string.Empty;
		string script = $"dotnet pack --configuration {Settings.BuildConfiguration} {includeSource} \"{Settings.ProjectFolder}/{Settings.ProjectName}.csproj\"";
		if (!await RunAsync(script))
			throw new Exception("Error build project!");

	}



}
