using Sabatex.Publish;
using sabatex_publish;

bool setup=false;
string? projFile;

void PostBuild(SabatexSettings config)
{
	File.WriteAllLines($"{config.TempPublishProjectFolder}/{config.ProjectName}.service", config.Linux.Service.GetConfig());
	File.WriteAllLines($"{config.TempPublishProjectFolder}/{config.ProjectName}", config.Linux.NGINX.GetConfig());
	if (config.Linux.NGINX.SSLPrivateLocalPath != null)
		File.Copy(config.Linux.NGINX.SSLPrivateLocalPath, $"{config.TempPublishProjectFolder}\\{config.Linux.NGINX.SSLPrivateName}.key");
	if (config.Linux.NGINX.SSLPublicLocalPath != null)
		File.Copy(config.Linux.NGINX.SSLPublicLocalPath, $"{config.TempPublishProjectFolder}\\{config.Linux.NGINX.SSLPublicName}.crt");

}


#region analize args
if (args.Any(s=> s.ToLower() == "--setup"))
{
	setup = true;
}

projFile = args.FirstOrDefault(s => s.ToLower() != "--setup");

if (projFile == null)
{
	var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj");
	if (files.Length == 0)
		throw new Exception("The Current directory must contains  *.csproj file");
	if (files.Length > 1)
		throw new Exception("The Current directory must contains only one *.csproj file");
	projFile = files[0];
}

if (!File.Exists(projFile))
{
	throw new FileNotFoundException("The file not exist: ", projFile);
}

#endregion





try
{
	var config = new SabatexSettings(projFile,setup);
	var ss = new ScriptShell(config);
	if (config.IsLibrary)
	{
		string nugetAuthToken = config.NUGET.GetToken();
		
		var localSS = new LocalScriptShell(config);
		localSS.Delete($"{config.OutputPath}\\*.nupkg");
		await localSS.PackAsync();

		string symbols = config.IsPreRelease ? ".symbols" : string.Empty;
		
		if (config.IsPreRelease)
		{
			if (!await localSS.RunAsync($"nuget add \"{config.OutputPath}\\{config.ProjectName}.{config.Version}.symbols.nupkg\" -source {config.NUGET.GetLocalStorage()}"))
			{
				throw new Exception("Error publish to nuget!");
			}

		}
		else
		{
			if (!await localSS.RunAsync($"dotnet nuget push \"{config.OutputPath}\\*{symbols}.nupkg\" -k {nugetAuthToken} -s https://api.nuget.org/v3/index.json --skip-duplicate"))
			{
				throw new Exception("Error publish to nuget!");
			}
		}
	}
	else
	{
		ss.Build();
		PostBuild(config);
		ss.PutTolinux();
		if (!config.Linux.FrontEnd)
			ss.UpdateBackend();
		else
			ss.UpdateBlazorwasm();

	}
	Console.WriteLine("Done!");
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
	Environment.Exit(1);
}

