using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Models.Base;
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
        public ApiException(ExceptionsTypes ErrorCode, string message, HttpStatusCode? statusCode = null) : base(ErrorCode, message)
        {
            Setup();
            this.StatusCode = (statusCode == null) ? (int)HttpStatusCode.BadRequest : (int)statusCode;
            this.Errors = new List<string> { message };
            this.Validations = null;
        }

        public ApiException(ExceptionsTypes ErrorCode, IEnumerable<IdentityError> errors, HttpStatusCode? statusCode = null) : base(ErrorCode, "Identity Error")
        {
            Setup();
            this.StatusCode = (statusCode == null) ? (int)HttpStatusCode.BadRequest : (int)statusCode;
            this.Errors = errors.Select(a => $"Error: {a.Code} - {a.Description}").ToList();
            this.Validations = null;
        }


        public ApiException(ExceptionsTypes ErrorCode, Exception ex, HttpStatusCode? statusCode = null) : base(ExceptionErrorCode(ex, ErrorCode), ExceptionErrorToString(ex))
        {
            Setup();
            this.StatusCode = (statusCode == null) ? (int)HttpStatusCode.BadRequest : (int)statusCode;
            this.Errors = new List<string> { ExceptionErrorToString(ex) };
            this.Validations = null;
        }

        public static string ExceptionErrorToString(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }

        public static ExceptionsTypes ExceptionErrorCode(Exception ex, ExceptionsTypes ErrorCode)
        {
            if(ex is DbUpdateException)
            {
                return ExceptionsTypes.DatabaseError;
            }else if(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                return ExceptionsTypes.DatabaseError;
            }

            return ErrorCode;

        }

        public ApiException(ExceptionsTypes ErrorCode, ModelStateDictionary modelState, ViewModelBase viewModel = null, HttpStatusCode? statusCode = null) : base(ErrorCode, "Model State Invalid")
        {
            Setup();
            this.StatusCode = (statusCode == null) ? (int)HttpStatusCode.BadRequest : (int)statusCode;
            this.Errors = ModelStateDictionaryToString(modelState);
            this.Validations = (viewModel == null) ? null: viewModel.Validations;

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
