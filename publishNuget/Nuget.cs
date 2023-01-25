using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publishNuget;

public class Nuget
{
    /// <summary>
    /// version publish to nuget major.minor.point
    /// if the last number not eq 0 the stage is only release
    /// if change version in csproj file  and last number eq then stage reset to alpha build 
    /// </summary>
    public string Version { get; set; } = "1.0.0";
    /// <summary>
    /// last success build version 
    /// </summary>
    public int BuildVersion { get; set; }

    public string Stage { get; set; } = BuildStages.Alpha;

    public string? ProjVersion { get; set; }

    public string BuildConfiguration => Stage == BuildStages.Release ? "Release" : "Debug";
    public override string ToString()
    {
        if (Stage == BuildStages.Release)
            return $"{Version}";
        else
            return $"{Version}-{Stage}{BuildVersion}";
    }
}

public static class BuildStages
{
    public const string Release = "release";
    public const string Alpha = "alpha";
    public const string Beta = "beta";
    public const string Rc = "rc";
}