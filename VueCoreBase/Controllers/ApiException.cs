using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Controllers.Exceptions
{
    public class ApiException : BaseException
    {
        public ApiException(ExceptionsTypes ErrorCode, string message) : base(ErrorCode, message)
        {
            this.StatusCode = (int)HttpStatusCode.BadRequest;
            this.ContentType = @"application/json";
        }

        public ApiException(ExceptionsTypes ErrorCode, string message, HttpStatusCode statusCode) : base(ErrorCode, message)
        {
            this.StatusCode = (int)statusCode;
            this.ContentType = @"application/json";
        }
    }
}
