using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Publish;

public class NGINX
{
    public string? SSLPrivate { get; set; }
    public string? SSLPublic { get; set; }
    public string[]? HostName { get; set; }
    public string? HostNames => string.Join(",", HostName ?? new string[] { });

    public int AppPort { get; set; } = 5000;

}
