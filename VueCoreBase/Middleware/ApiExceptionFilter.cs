using Controllers.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Exception
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private void LogError(ExceptionContext context)
        {
            var exception = context.Exception;

            while(exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            string[] errorMessage =
            { $"##########################################################################",
                $"Error: {exception.ToString()}",
                $"Message: {exception.Message}",
                $"Error Thrown in: {exception.TargetSite}",
                $"Stack Trace: {exception.StackTrace}",
                " ##########################################################################" };

            File.AppendAllLines("errorlog.txt", errorMessage);
        }

        public override void OnException(ExceptionContext context)
        {
            
            ApiError apiError = null;
            if (context.Exception is ApiException)
            {
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(ex.Message);
                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;
                //
                // handle logging here
            }
            else
            {
                return;
                /*
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
                
                apiError = new ApiError(msg);
                apiError.detail = stack;
                context.HttpContext.Response.StatusCode = 500;
                */
               
            }
            // always return a JSON result
            context.Result = new JsonResult(apiError);
            base.OnException(context);
        }
    }
}