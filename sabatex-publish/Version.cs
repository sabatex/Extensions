using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Publish;

internal struct Version
{
    public readonly string Major;
    public readonly string Minor;
    public readonly string Point;
    public readonly bool IsPreRelease;
    public Version(string? value)
    {
        if (value== null) throw new ArgumentNullException("value");
        var v = value.Split('.');
        if (v.Length != 3) throw new Exception($"The version {value} must have kind *.*.*");
        if (int.TryParse(v[0], out int major))
            Major = v[0];
        else
            throw new Exception($"The {v[0]} is not valid number in version {value} ");
        
        if (int.TryParse(v[1], out int minor))
            Minor= v[1];
        else
            throw new Exception($"The {v[1]} is not valid number in version {value} ");
        Point= v[2];
        if (int.TryParse(v[2], out int point))
        {
            
            IsPreRelease = false;
        }
        else
        {
            if (Point.Length <2)
                throw new Exception($"The {v[2]} is not valid point prerelease in version {value} ");
            IsPreRelease = true;
        }
    }
            

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Point}";
    }
}
