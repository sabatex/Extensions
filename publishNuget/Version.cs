using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publishNuget
{
    internal struct Version
    {
        public int Major;
        public int Minor;
        public int Point;
        public Version(string? value)
        {
            if (value== null) throw new ArgumentNullException("value");
            var v = value.Split('.');
            if (v.Length != 3) throw new Exception($"The version {value} must have kind *.*.*");
            if (int.TryParse(v[0], out int major))
                Major = major;
            else
                throw new Exception($"The {v[0]} is not valid number in version {value} ");
            
            if (int.TryParse(v[1], out int minor))
                Minor=minor;
            else
                throw new Exception($"The {v[1]} is not valid number in version {value} ");
            
            if (int.TryParse(v[2], out int point))
                Point=point;
            else
                throw new Exception($"The {v[2]} is not valid number in version {value} ");
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Point}";
        }
    }
}
