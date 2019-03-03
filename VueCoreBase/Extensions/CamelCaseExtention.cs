using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions.CamelCase
{
    public static class CamelCaseExtention
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }

    }
}
