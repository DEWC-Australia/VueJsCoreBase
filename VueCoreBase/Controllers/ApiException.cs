using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.VeeValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Controllers.Exceptions
{
    public class ApiException : BaseException
    {
        private void Setup()
        {
            this.StatusCode = (int)HttpStatusCode.BadRequest;
            this.ContentType = @"application/json";
        }
        public ApiException(ExceptionsTypes ErrorCode, string message, Dictionary<string, ClassProperty> validations = null) : base(ErrorCode, message)
        {
            Setup();
            this.Errors = new List<string> { message };
            this.Validations = validations;
        }

        public ApiException(ExceptionsTypes ErrorCode, string message, HttpStatusCode statusCode, Dictionary<string, ClassProperty> validations = null) : base(ErrorCode, message)
        {
            Setup();
            this.Errors = new List<string> { message };
            this.Validations = validations;
        }

        public ApiException(ExceptionsTypes ErrorCode, IEnumerable<IdentityError> errors, Dictionary<string, ClassProperty> validations = null) : base(ErrorCode, "Identity Error")
        {
            Setup();
            this.Errors = errors.Select(a => $"Error: {a.Code} - {a.Description}").ToList();
            this.Validations = validations;
        }


        public ApiException(ExceptionsTypes ErrorCode, Exception ex, Dictionary<string, ClassProperty> validations = null) : base(ErrorCode, ExceptionErrorToString(ex))
        {
            Setup();
            this.Errors = new List<string> { ExceptionErrorToString(ex) };
            this.Validations = validations;
        }

        public static string ExceptionErrorToString(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }

        public ApiException(ExceptionsTypes ErrorCode, ModelStateDictionary modelState, Dictionary<string, ClassProperty> validations = null) : base(ErrorCode, "Model State Invalid")
        {
            Setup();
            this.Errors = ModelStateDictionaryToString(modelState);
            this.Validations = validations;
        }

        public static List<string> ModelStateDictionaryToString(ModelStateDictionary modelState)
        {
            var output = new List<string>();
           
            foreach (var error in modelState.ToList())
            {
                output.AddRange(error.Value.Errors.Select(a => $"Error: {error.Key} - {a.ErrorMessage}").ToList());   
            }

            return output;
        }
    }
}
