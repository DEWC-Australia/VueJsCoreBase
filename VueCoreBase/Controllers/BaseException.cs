using Models.VeeValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Controllers.Exceptions
{
    public class BaseException : Exception
    {
        public List<string> Errors { get; set; }
        public Dictionary<string, ClassProperty> Validations { get; set; }
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";
        public ExceptionsTypes ExceptionType { get; set; }
        public BaseException(ExceptionsTypes ErrorCode, string Message): base(String.Format("{0} ({1})", Message, GetDescription(ErrorCode)))
        {
            ExceptionType = ErrorCode;
            if(ExceptionType == ExceptionsTypes.DatabaseError || ExceptionType == ExceptionsTypes.SystemError)
                StatusCode = (int)HttpStatusCode.InternalServerError;
        }

       

        public static string GetDescription(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumValue.ToString();
        }
    }
}
