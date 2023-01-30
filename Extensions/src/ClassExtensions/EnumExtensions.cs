﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sabatex.Extensions.ClassExtensions
{
    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default(DescriptionAttribute)
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? string.Empty
                    : descriptionAttribute.Description;
        }

        public static string GetDescription(this Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default(DescriptionAttribute)
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Description;
        }
        public static Tuple<string, string>[] GetEnumDisplayName(this Enum _enum)
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            var enumList = Enum.GetValues(_enum.GetType());
            foreach (var e in enumList)
            {
                result.Add(Tuple.Create(e.ToString(), ((Enum)e).GetDescription()));
            }

            return result.ToArray();


        }
        public static Tuple<Enum, string>[] GetEnumListWithDescription(Type _enum)
        {
            List<Tuple<Enum, string>> result = new List<Tuple<Enum, string>>();
            var enumList = Enum.GetValues(_enum);
            foreach (var e in enumList)
            {
                var value = (Enum)e;
                string description = value.Description();
                if (description != "")
                    result.Add(new Tuple<Enum, string>(value, description));
            }
            return result.ToArray();
        }


    }

}
