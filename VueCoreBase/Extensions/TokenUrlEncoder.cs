using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Extensions.TokenUrlEncoder
{
    public static class TokenUrlEncoder
    {
        public static string TokenEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static string TokenDecode(this string str)
        {
            return HttpUtility.UrlDecode(str); ;
        }
    }
}
