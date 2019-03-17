using Controllers.Exceptions;
using Data.DatabaseLogger;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Middleware.Logger
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private int ErrorLevel = 400;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private string UserName(HttpContext context)
        {
            string userName = (context.User.Identity == null && context.User.Identity.Name == null) ? "Anonyomous" : context.User.Identity.Name;

            string authHeader = context.Request.Headers["Authorization"];

            if (userName == null && authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                // Split username and password
                userName = decodedUsernamePassword.Split(':', 2)[0];
            }

            return userName;
        }

        public async Task Invoke(HttpContext context, DatabaseLoggerContext db)
        {

            bool containsBasePath = context.Request.Path.ToString().ToLower().Contains("caldav");
            bool containsWellknownPath = context.Request.Path.ToString().ToLower().Contains(".well-known/caldav");

            // can log the user now 
            string user = UserName(context);
            // stream to restore the request body
            MemoryStream injectedRequestStream = new MemoryStream();

            //Copy a pointer to the original response body stream
            Stream originalBodyStream = context.Response.Body;

            string userAgent = "";

            try
            {
                //First, get the incoming request
                var request = await FormatRequest(context.Request);

                var bytesToWrite = Encoding.UTF8.GetBytes(request["Body"]);
                injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                injectedRequestStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = injectedRequestStream;


                //Create a new memory stream...
                using (var responseBody = new MemoryStream())
                {
                    //...and use that for the temporary response body
                    context.Response.Body = responseBody;

                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);

                    //Format the response from the server
                    var response = await FormatResponse(context.Response);

                    //TODO: Save log to chosen datastore

                    //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                    await responseBody.CopyToAsync(originalBodyStream);



                    // caldav log
                    if ((containsBasePath || containsWellknownPath) && context.Response.StatusCode >= ErrorLevel)
                    {
                        StringBuilder requestText = new StringBuilder();
                        StringBuilder responseText = new StringBuilder();
                        requestText.AppendLine("");
                        requestText.AppendLine("#######################################################");
                        requestText.AppendLine($"Start - {((DateTime)request["Start"]).ToString()}");
                        requestText.AppendLine($"REQUEST HttpMethod: {request["Method"]}, Path: {request["Path"]}");
                        requestText.AppendLine($"Headers:");
                        Dictionary<string, string> headers = request["Headers"];

                        userAgent = (headers.ContainsKey("User-Agent")) ? headers["User-Agent"] : "";

                        foreach (var header in headers)
                        {
                            requestText.AppendLine($"{header.Key}: {header.Value}");
                        }
                        requestText.AppendLine($"Body :");
                        requestText.AppendLine(request["Body"]);

                        responseText.AppendLine("");
                        responseText.AppendLine("#######################################################");
                        responseText.AppendLine($"Http/1.1: {response["StatusCode"]}");
                        responseText.AppendLine($"Content-Type: {context.Response.ContentType}");
                        headers = response["Headers"];

                        foreach (var header in headers)
                        {
                            responseText.AppendLine($"{header.Key}: {header.Value}");
                        }
                        responseText.AppendLine($"Body :");
                        responseText.AppendLine(request["Body"]);

                        responseText.AppendLine($"Stop - {((DateTime)response["Stop"]).ToString()}");
                        responseText.AppendLine("");
                        responseText.AppendLine("#######################################################");
                        responseText.AppendLine("");

                        string responseContentType = null;

                        if (response.ContainsKey("ContentType") && response["ContentType"] != null)
                            responseContentType = (response["ContentType"] as string).Length > 255 ? (response["ContentType"] as string).Substring(0, 255) : (response["ContentType"] as string);

                        await db.CalDavLog.AddAsync(new CalDavLog
                        {
                            Method = request["Method"],
                            Path = (request["Path"] as string).Length > 255 ? (request["Path"] as string).Substring(0, 255) : (request["Path"] as string),
                            Start = request["Start"],
                            Stop = response["Stop"],
                            UserAgent = userAgent.Length > 255 ? userAgent.Substring(0, 255) : userAgent,
                            ResponseContentType = responseContentType,
                            StatusCode = response["StatusCode"],
                            Response = responseText.ToString(),
                            Request = requestText.ToString()
                        });
                    }

                    userAgent = context.Request.Headers.ContainsKey("User-Agent") ? context.Request.Headers["User-Agent"].First() : "";
                    await db.RequestLog.AddAsync(new RequestLog
                    {
                        Method = request["Method"],
                        Path = (request["Path"] as string).Length > 255 ? (request["Path"] as string).Substring(0, 255) : (request["Path"] as string),
                        Start = request["Start"],
                        Stop = response["Stop"],
                        UserAgent = userAgent.Length > 255 ? userAgent.Substring(0, 255) : userAgent,
                        StatusCode = response["StatusCode"],
                        UserName = user
                    });
                }

                await db.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApiException(ExceptionsTypes.LoggingMiddlewareError, ex);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
                injectedRequestStream.Dispose();
            }

        }

        private async Task<Dictionary<string, dynamic>> FormatRequest(HttpRequest request)
        {
            Dictionary<string, dynamic> requestLog = new Dictionary<string, dynamic>();
            requestLog.Add("Start", DateTime.Now);
            requestLog.Add("Method", request.Method);
            requestLog.Add("Path", $"{request.Scheme} {request.Host}{request.Path} {request.QueryString}");

            Dictionary<string, string> headerLog = new Dictionary<string, string>();
            foreach (var header in request.Headers.ToList())
            {
                headerLog.Add(header.Key, header.Value);
            }
            requestLog.Add("Headers", headerLog);


            using (var bodyReader = new StreamReader(request.Body))
            {

                string bodyText = await bodyReader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(bodyText) == false)
                {
                    requestLog.Add("Body", bodyText);
                }
                else
                {
                    requestLog.Add("Body", "");
                }
            }

            return requestLog;
        }

        private async Task<Dictionary<string, dynamic>> FormatResponse(HttpResponse response)
        {
            Dictionary<string, dynamic> responseLog = new Dictionary<string, dynamic>();
            responseLog.Add("StatusCode", response.StatusCode);
            responseLog.Add("ContentType", response.ContentType);

            Dictionary<string, string> headerLog = new Dictionary<string, string>();
            foreach (var header in response.Headers.ToList())
            {
                headerLog.Add(header.Key, header.Value);
            }

            responseLog.Add("Headers", headerLog);


            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string bodyText = await new StreamReader(response.Body).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(bodyText) == false)
            {
                responseLog.Add("Body", bodyText);
            }
            else
            {
                responseLog.Add("Body", "");
            }

            responseLog.Add("Stop", DateTime.Now);
            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return responseLog;
        }
    }
}
