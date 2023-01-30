using sabatex.TaxUA.CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Linq;

namespace sabatex.TaxUA
{
    public static class StringExtension
    {
        public static DateTime? GetTAXDate(this string value)
        {
            value = value.Trim();
            if (value == "") return null;
            return DateTime.Parse(value.Substring(0, 2) + "." + value.Substring(2, 2) + "." + value.Substring(4));
        }
        public static int? GetAsInt(this XmlElement value)
        {
            if (value == null) return new int?();
            string s = value.InnerText.Trim();
            if (value.InnerText.Length == 0) return new int?();
            try
            {
                return new int?(int.Parse(s));
            }
            catch
            {
                Trace.WriteLine("Error parse " + s + " to int!!!");
                return new int?();
            }
        }
        public static uint? GetAsUint(this string value)
        {
            if (value == null) return new uint?();
            string s = value.Trim();
            if (s.Length == 0) return new uint?();
            try
            {
                return new uint?(uint.Parse(s));
            }
            catch
            {
                Trace.WriteLine("Error parse " + s + " to int!!!");
                return new uint?();
            }
        }

        public static string GetAsString(this XmlElement value)
        {
            if (value == null) return "";
            return value.InnerText.Trim();
        }
        public static DGPNtypr? GetAsDGPNtypr(this XmlElement value)
        {
            int? lHTYPR = value.GetAsInt();
            return lHTYPR != null?(DGPNtypr)lHTYPR.Value:new DGPNtypr?();
        }
        public static ulong? GetAsUlong(this string value)
        {
            if (value == null) return new ulong?();
            string s = value.Trim();
            if (s.Length == 0) return new ulong?();
            try
            {
                return new ulong?(ulong.Parse(s));
            }
            catch
            {
                Trace.WriteLine("Error parse " + s + " to ulong!!!");
                return new ulong?();
            }
        }
        public static ulong? GetAsUlong(this XmlElement value) => value == null ? new ulong?() : value.InnerText.GetAsUlong();



        public static decimal? GetAsDecimal(this string value)
        {
            if (value == null) return new decimal?();
            string s = value.Trim();
            if (s.Length == 0) return new decimal?();
            try
            {
                return new decimal?(Decimal.Parse(s));
            }
            catch
            {
                try
                {
                    return new decimal?(Decimal.Parse(s.Replace(".", ",")));
                }
                catch
                {
                    Trace.WriteLine("Error parse " + s + " to decimal!!!");
                    return new decimal?();
                }
            }

        }
        public static decimal? GetAsDecimal(this XmlElement value)=> value == null ? new decimal?() : value.InnerText.GetAsDecimal();


        /// <summary>
        /// Get digits symbols from string 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DigitalSubstring(this string s)
        {
            StringBuilder result = new StringBuilder();
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(s[i]))
                {
                    return result.ToString().Reverse().ToString();
                }
                result.Append(s[i]);
            }

            return result.ToString();
        }

    }
}
