using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Logger
{
    public class Logging
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<Logging> _logger;

        public Logging(RequestDelegate next, ILogger<Logging> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string method = context.Request.Method;
            string path = context.Request.Path;

            await _next.Invoke(context);

            int statusCode = context.Response.StatusCode;

            _logger.LogTrace($"Method: {method} Path: {path} StatusCode: {statusCode}" );
        }
    }
}
