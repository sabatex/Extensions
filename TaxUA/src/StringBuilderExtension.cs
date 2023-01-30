using sabatex.TaxUA.CommonTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;

namespace sabatex.TaxUA
{
    /// <summary>
    /// Adds IndexOf, IsStringAt, AreEqual, and Substring to all StringBuilder objects.
    /// </summary>
    public static class StringBuilderExtension
    {
        const int NOT_FOUND = -1;
        //const string XsiNil = " xsi:nil=\"true\" /";
        static NumberFormatInfo NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "." };
        // Adds IndexOf, Substring, AreEqual to the StringBuilder class.
        public static int IndexOf(this StringBuilder theStringBuilder, string value)
        {
            return IndexOf(theStringBuilder, value, 0);
        }
        public static int IndexOf(this StringBuilder theStringBuilder, string value, int startPosition)
        {
            if (theStringBuilder == null)
            {
                return NOT_FOUND;
            }
            if (String.IsNullOrEmpty(value))
            {
                return NOT_FOUND;
            }
            int count = theStringBuilder.Length;
            int len = value.Length;
            if (count < len)
            {
                return NOT_FOUND;
            }
            int loopEnd = count - len + 1;
            if (startPosition >= loopEnd)
            {
                return NOT_FOUND;
            }
            for (int loop = startPosition; loop < loopEnd; loop++)
            {
                bool found = true;
                for (int innerLoop = 0; innerLoop < len; innerLoop++)
                {
                    if (theStringBuilder[loop + innerLoop] != value[innerLoop])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return loop;
                }
            }
            return NOT_FOUND;
        }
        public static int IndexOf(this StringBuilder theStringBuilder, char value, int startPosition)
        {
            if (theStringBuilder == null)
            {
                return NOT_FOUND;
            }
            int count = theStringBuilder.Length;

            if (startPosition >= count)
            {
                return NOT_FOUND;
            }
            for (int loop = startPosition; loop < count; loop++)
            {
                if (theStringBuilder[loop] == value) return loop;
            }
            return NOT_FOUND;
        }
        public static int IndexOf(this StringBuilder theStringBuilder, char value)
        {
            return IndexOf(theStringBuilder, value, 0);
        }
        public static string Substring(this StringBuilder theStringBuilder, int startIndex, int length)
        {
            return theStringBuilder == null ? null : theStringBuilder.ToString(startIndex, length);
        }
        public static bool AreEqual(this StringBuilder theStringBuilder, string compareString)
        {
            if (theStringBuilder == null)
            {
                return compareString == null;
            }
            if (compareString == null)
            {
                return false;
            }
            int len = theStringBuilder.Length;
            if (len != compareString.Length)
            {
                return false;
            }
            for (int loop = 0; loop < len; loop++)
            {
                if (theStringBuilder[loop] != compareString[loop])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Compares one string to part of another string.
        /// </summary>
        /// <param name="haystack"></param>
        /// <param name="needle">Needle to look for</param>
        /// <param name="position">Looks to see if the needle is at position in haystack</param>
        /// <returns>Substring(theStringBuilder,offset,compareString.Length) == compareString</returns>
        public static bool IsStringAt(this StringBuilder haystack, string needle, int position)
        {
            if (haystack == null)
            {
                return needle == null;
            }
            if (needle == null)
            {
                return false;
            }
            int len = haystack.Length;
            int compareLen = needle.Length;
            if (len < compareLen + position)
            {
                return false;
            }
            for (int loop = 0; loop < compareLen; loop++)
            {
                if (haystack[loop + position] != needle[loop])
                {
                    return false;
                }
            }
            return true;
        }


        static StringBuilder addNullValue(this StringBuilder st, string name)
        {
            return st.AppendLine("<" + name + " xsi:nil=\"true\" />");
        }
        static StringBuilder addValue(this StringBuilder st,string name,string value)
        {
            return st.AppendLine("<" + name + ">" + value + "</" + name + ">");
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, string value)
        {
            return value == null ? st.addNullValue(token):st.addValue(token, SecurityElement.Escape(value));
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, int? value)
        {
            return value == null ? st.addNullValue(token) : st.addValue(token, value.ToString());
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, int value)
        {
            return st.addValue(token, value.ToString());
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, int value,string format)
        {
            return st.addValue(token, value.ToString(format));
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, ulong? value)
        {
            return value == null ? st.addNullValue(token) : st.addValue(token, value.ToString());
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, ulong value)
        {
            return st.addValue(token, value.ToString());
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, uint? value)
        {
            return value == null ? st.addNullValue(token) : st.addValue(token, value.ToString());
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, uint value)
        {
            return st.addValue(token, value.ToString());
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, decimal? value)
        {
            return value == null ? st.addNullValue(token) : st.addValue(token, value.ToString());
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, decimal value)
        {
            return st.addValue(token, value.ToString());
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, DGPNtypr? value)
        {
            return value == null ? st.addNullValue(token) : st.addValue(token, ((int)value).ToString("00"));
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, DGPNtypr value)
        {
            return st.addValue(token, ((int)value).ToString("00"));
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, DateTime? value)
        {
            return value == null ? st.addNullValue(token) : st.addValue(token, value.Value.DateToOPZFormat());
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, DateTime value)
        {
            return st.addValue(token, value.DateToOPZFormat());
        }

        static StringBuilder addNullValue(this StringBuilder st, string name, int line)
        {
            return st.AppendLine("<" + name + " ROWNUM=\"" + line.ToString() + "\" " + " xsi:nil=\"true\" />");
        }
        static StringBuilder addValue(this StringBuilder st, string name, string value,int line)
        {
            return st.AppendLine("<" + name + " ROWNUM=\"" + line.ToString() + "\">" + value + "</" + name + ">");
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, string value, int line)
        {
            return value == null ? st.addNullValue(token, line) : st.addValue(token, SecurityElement.Escape(value), line);
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, decimal? value, int line)
        {
            return value == null ? st.addNullValue(token, line) : st.addValue(token, value.ToString(), line);
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, decimal value, int line)
        {
            return st.addValue(token, value.ToString(), line);
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, decimal? value, string format, int line)
        {
            return value == null ? st.addNullValue(token, line) : st.addValue(token, value.Value.ToString(format, NumberFormat), line);
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, decimal value, string format, int line)
        {
            return st.addValue(token, value.ToString(format, NumberFormat), line);
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, EVAT? value, int line)
        {
            return value==null? st.addNullValue(token, line) : st.addValue(token, ((int)value).ToString(), line);
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, EVAT value, int line)
        {
           return st.addValue(token,((int)value).ToString(),line);
        }

        public static StringBuilder AddValue(this StringBuilder st, string token, int? value,string format, int line)
        {
            return value == null ? st.addNullValue(token, line) : st.addValue(token, value.Value.ToString(format), line);
        }
        public static StringBuilder AddValue(this StringBuilder st, string token, int? value, int line)
        {
            return value == null ? st.addNullValue(token, line) : st.addValue(token, value.Value.ToString(), line);
        }

        /// <summary>
        /// Дата в формате ддммРРРР
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateToOPZFormat(this DateTime dt)
        {
            return dt.Day.ToString().PadLeft(2, '0') + dt.Month.ToString().PadLeft(2, '0') + dt.Year.ToString().PadLeft(4, '0');
        }

    }
}
