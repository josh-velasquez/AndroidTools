using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AndroidTools.Utils
{
    static class EnumUtil
    {
        public static string GetEnumDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }
    }
}
