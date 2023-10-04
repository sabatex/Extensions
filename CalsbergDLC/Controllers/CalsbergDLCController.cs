using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CalsbergDLC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalsbergDLCController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<CalsbergDLCController> _logger;

        public CalsbergDLCController(ILogger<CalsbergDLCController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private bool Run(string script, string? workingDirectory = null)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = $"/C {script}";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                proc.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            }
            else
            {
                proc.StartInfo.WorkingDirectory = workingDirectory;
            }
            proc.OutputDataReceived += (a, b) => Console.WriteLine(b.Data);
            proc.ErrorDataReceived += (a, b) => Console.WriteLine(b.Data);
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();
            return proc.ExitCode == 0 || proc.ExitCode == 1000;
        }
        [HttpPost("CRMData")]
        public async Task<string> PostCRMData([FromQuery]string crmType)
        {
            string crm = await new StreamReader(Request.Body).ReadToEndAsync();
            string fileName = $"{crmType}.xml";
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);    
            }
            await System.IO.File.WriteAllTextAsync(crm, fileName);
            if (Run($"dlc.exe -url https://crmtest.carlsberg.ua/xDataLink/xDataLink.asmx -in {fileName} - out Log{fileName}"))
            {
                return await System.IO.File.ReadAllTextAsync($"Log{fileName}");
            }
            return "";
        }


    }
}