using Controllers.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;

namespace Middleware.Exception
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private ILogger _Logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _Logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _Logger.LogInformation("Api Exception Filter Called");
            ApiError apiError = null;

            if (context.Exception is ApiException apiEx)
            {
                _Logger.LogInformation("Api Exception found");

                apiError = new ApiError(apiEx);

                var ex = context.Exception as ApiException;

                context.HttpContext.Response.StatusCode = ex.StatusCode;

                if (ex.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    StringBuilder message = new StringBuilder();
                    message.Append("Internal Server Error: ");
                    foreach(var error in ex.Errors)
                    {
                        message.AppendLine($"{error}");
                    }
                    _Logger.Log(LogLevel.Critical, ex, message.ToString());
                }
                    

                context.Exception = null;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                _Logger.LogInformation("Unauthorized Access Exception found");

                apiError = new ApiError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;

                _Logger.Log(LogLevel.Critical, context.Exception, "Unauthorized Access");

            }
            else
            {
                int statusCode = (context.HttpContext.Response.StatusCode == 200) ? 500 : context.HttpContext.Response.StatusCode;

                _Logger.Log(LogLevel.Critical, context.Exception, $"Unhandled Exception - Path:{context.HttpContext.Request.Scheme} {context.HttpContext.Request.Host}{context.HttpContext.Request.Path} {context.HttpContext.Request.QueryString}, StatusCode:{ statusCode}");

                return;

            }
            // always return a JSON result
            context.Result = new JsonResult(apiError);
            base.OnException(context);
        }
    }
}