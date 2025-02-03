using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sabatex_publish;

public class AppConfig
{
    public Dictionary<string, string> ConnectionStrings { get; set; }

    public string? GetConfig()
    {
        return System.Text.Json.JsonSerializer.Serialize(this as AppConfig);
    }

}
