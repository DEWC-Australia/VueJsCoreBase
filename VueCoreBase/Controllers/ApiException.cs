using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public ApiException(ExceptionsTypes ErrorCode, IEnumerable<IdentityError> errors) : base(ErrorCode, IdentityErrorToString(errors))
        {
            this.StatusCode = (int)HttpStatusCode.BadRequest;
            this.ContentType = @"application/json";
        }

        public static string IdentityErrorToString(IEnumerable<IdentityError> errors)
        {
            StringBuilder message = new StringBuilder();
            foreach(var error in errors.ToList())
            {
                message.AppendLine(String.Format("Error: {0} - {1}", error.Code, error.Description));
            }

            return message.ToString();

        }

        public ApiException(ExceptionsTypes ErrorCode, Exception ex) : base(ErrorCode, ExceptionErrorToString(ex))
        {
            this.StatusCode = (int)HttpStatusCode.BadRequest;
            this.ContentType = @"application/json";
        }

        public static string ExceptionErrorToString(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }

        public ApiException(ExceptionsTypes ErrorCode, ModelStateDictionary modelState) : base(ErrorCode, ModelStateDictionaryToString(modelState))
        {
            this.StatusCode = (int)HttpStatusCode.BadRequest;
            this.ContentType = @"application/json";
        }

        public static string ModelStateDictionaryToString(ModelStateDictionary modelState)
        {
            StringBuilder message = new StringBuilder();
            foreach (var error in modelState.ToList())
            {
                StringBuilder errors = new StringBuilder();
                foreach(var keyError in error.Value.Errors.ToList())
                {
                    message.AppendLine(String.Format("Error: {0} - {1}", error.Key, keyError.ErrorMessage));
                }
                
            }
           
            return message.ToString();
        }
    }
}
