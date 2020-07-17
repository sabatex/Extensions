using System.Text;
using System.Collections.Specialized;
using System;
using System.Globalization;

namespace sabatex.Extensions.ClassExtensions
{
    public static class StringExtension
    {
        public static string Parse(this string value, char delimiter)
        {
            if (value.Trim().Length == 0) return "";
            string result = value.Substring(0, value.IndexOf(delimiter)).Trim();
            return result;
        }
        public static string Parse(this string value)
        {
            // Parse on default delimiter
            return value.Parse(';');
        }
        public static bool CheckString(this string value, string check)
        {
            foreach (char c in value)
            {
                if (check.IndexOf(c) == -1) return false;
            }
            return true;
        }
        /// <summary>
        /// Разделение строки на подстроки с разделителем delimiter 
        /// </summary>
        /// <param name="str">Строка с разделителями </param>
        /// <param name="delimiter">Символ разделитель</param>
        /// <returns>Коллекция строк</returns>
        public static StringCollection ToStringCollection(this string str, char delimiter = ';')
        {
            StringCollection result = new StringCollection();
            int i = 0;
            while (i < str.Length)
            {
                int pos = str.IndexOf(delimiter, i);
                if (pos == -1)                                          // End
                {
                    // last substring
                    result.Add(str.Substring(i).Trim());
                    return result;
                }
                result.Add(str.Substring(i, pos - i).Trim());
                i = pos + 1;
            }
            return result;
        }

        public static string ToRussian(this string value)
        {
            StringBuilder s = new StringBuilder(value);
            for (int i = 0;i < s.Length;i++)
            {
                s[i] = s[i].UpperKeyToRus();
            }
            return s.ToString();
        }
        public static string ToUkrainian(this string value)
        {
            StringBuilder s = new StringBuilder(value);
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = s[i].UpperKeyToUkraine();
            }
            return s.ToString();
        }


        public static string ToHeximal(this string value)
        {
            StringBuilder result = new StringBuilder();
            if (value != null)
            {
                foreach (char c in value)
                {
                    result.Append(((UInt16)c).ToString());
                    result.Append(',');
                }
            }
            return result.ToString();

        }

        public static string FromHeximal(this string value)
        {
            StringBuilder result = new StringBuilder();
            if (value!=null)
            {
                var st = value.ToStringCollection(',');
                foreach (string s in st)
                {
                    if (UInt16.TryParse(s, out ushort rs))
                        result.Append((char)rs);
                    else
                        throw new Exception("System Error !!!"); 
                }
            }
            return result.ToString();

        }

        /// <summary>
        /// Use . or , for decimal separator
        /// </summary>
        /// <param name="value">string with decimal simbols</param>
        /// <param name="result">decimal value</param>
        /// <returns>true is succes</returns>
        public static bool TryToDecimal(this string value,out decimal result)
        {
            if (Decimal.TryParse(value, out result)) return true;
            if (Decimal.TryParse(value.Replace('.', ','), out result)) return true;
            return false;

        }



    }
}
