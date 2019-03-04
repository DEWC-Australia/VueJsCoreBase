using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions.Base64Utility
{
    public static class Base64Utility
    {
        static readonly char[] padding = { '=' };

        public static string UrlSafeEncode(this string base64String)
        {
            return base64String.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }

        public static string UrlSafeDecode(this string urlSafeBase64String)
        {
            string base64String = urlSafeBase64String.Replace('_', '/').Replace('-', '+');
            switch (urlSafeBase64String.Length % 4)
            {
                case 2: base64String += "=="; break;
                case 3: base64String += "="; break;
            }
            return base64String;
        }
    }
}
