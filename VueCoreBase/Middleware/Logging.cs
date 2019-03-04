using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Data.VueCoreBase;

namespace Middleware.Logger
{
    public class ContextLogger
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<Logging> _logger;
       // private VueCoreBaseContext mDb;

        public ContextLogger(RequestDelegate next, ILogger<Logging> logger/*, VueCoreBaseContext db*/)
        {
            _next = next;
            _logger = logger;
           // mDb = db;
        }

        public async Task Invoke(HttpContext context)
        {
            string method = context.Request.Method;
            string path = context.Request.Path;

            await _next.Invoke(context);

            int statusCode = context.Response.StatusCode;

            _logger.LogTrace($"Method: {method} Path: {path} StatusCode: {statusCode}" );
            /*
            await mDb.Logging.AddAsync(new Logging
            {
                RequestMethod = method,
                RequestPath = path,
                ResponseStatusCode = statusCode
            });

            await mDb.SaveChangesAsync();
            */
        }
    }
}
