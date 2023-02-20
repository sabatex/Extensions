// See https://aka.ms/new-console-template for more information
using CommonLibrary;

try
{
    if (args.Length != 1)
        throw new Exception("The command line args must pass full path project file *.csproj");

    ProjectConfig config = new ProjectConfig(args[0]);
    var nugetKeyPath = config.GetValue<string?>("nugetAuthTokenPath");
    if (nugetKeyPath == null)
        throw new Exception("The parameter 'nugetAuthTokenPath'  must be defined in file SabatexSettings.json");

    string[] token = File.ReadAllLines(nugetKeyPath);
    if (token.Length == 0 || token.Length > 1)
        throw new Exception("The NUGET TOKEN is wrong!");
    string nugetAuthToken = token[0];

    config.RunScript($"del {config.OutputPath}\\*.nupkg");

    string includeSource = config.IsPreRelease ? "--include-source" : string.Empty;
    string script = $"dotnet pack --configuration {config.BuildConfiguration} {includeSource} \"{config.ProjectFilePath}\"";
    if (!config.RunScript(script))
        throw new Exception("Error build project!");

    string symbols = config.IsPreRelease ? ".symbols" : string.Empty;
    script = $"dotnet nuget push \"{config.OutputPath}\\*{symbols}.nupkg\" -k {nugetAuthToken} -s https://api.nuget.org/v3/index.json --skip-duplicate";
    if (!config.RunScript(script))
        throw new Exception("Error publish to nuget!");
    Console.WriteLine("Done!");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Environment.Exit(1);
}





