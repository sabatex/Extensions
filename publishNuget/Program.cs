using CommonLibrary;

namespace PublishNUGET;
internal class Program
{

    static void Main(string[] args)
    {
        try
        {
            var config = SabatexSettings.GetConfig(args);
            var ss = new ScriptShell(config);
            string projPath = string.Empty;


            string nugetAuthToken = config.NUGET.GetToken();

            ss.Run($"del {config.OutputPath}\\*.nupkg");

            string includeSource = config.IsPreRelease ? "--include-source" : string.Empty;
            string script = $"dotnet pack --configuration {config.BuildConfiguration} {includeSource} \"{config.ProjectFile}\"";
            if (!ss.Run(script))
                throw new Exception("Error build project!");
            string symbols = config.IsPreRelease ? ".symbols" : string.Empty;
            if (config.IsPreRelease)
            {
                script = $"nuget add \"{config.OutputPath}\\{config.ProjectName}.{config.Version}.symbols.nupkg\" -source {config.NUGET.GetLocalStorage()}";

            }
            else
            {
                script = $"dotnet nuget push \"{config.OutputPath}\\*{symbols}.nupkg\" -k {nugetAuthToken} -s https://api.nuget.org/v3/index.json --skip-duplicate";
            }
            if (!ss.Run(script))
                    throw new Exception("Error publish to nuget!");
  
            Console.WriteLine("Done!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }

    }
}





