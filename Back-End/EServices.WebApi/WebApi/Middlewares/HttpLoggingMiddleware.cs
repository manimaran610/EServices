using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Serilog.Context;


namespace WebApi.Middlewares
{
    /// <summary>
    /// Middleware for Logging Request and Responses.
    /// Remarks: Original code taken from https://exceptionnotfound.net/using-middleware-to-log-requests-and-responses-in-asp-net-core/
    /// </summary>
    public class HttpLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly List<string> _excludePaths = new() { "Log" };

        public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_excludePaths.Any(e => context.Request.Path.Value.Contains(e)))
            {
                //Continue down the Middleware pipeline
                await _next(context);
            }
            else
            {
                var start = Stopwatch.GetTimestamp();

                //Copy  pointer to the original response body stream
                var originalBodyStream = context.Response.Body;

                //Get incoming request
                var request = await GetRequestAsTextAsync(context.Request);
                //Log it
                // Serilog.Log.Information(request);


                //Create a new memory stream and use it for the temp reponse body
                await using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline
                await _next(context);

                //Format the response from the server
                var response = await GetResponseAsTextAsync(context.Response);
                //Log it
                // _logger.LogInformation(response);

                //Copy the contents of the new memory stream, which contains the response to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
                var elapsed = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                LogRequestResponse(context, elapsed, request, response);
            }
        }


        private void LogRequestResponse(HttpContext context, double elapsed, string request, string response)
        {
            LogContext.PushProperty("QueryString", context.Request.QueryString);
            LogContext.PushProperty("StatusCode", context.Response.StatusCode);
            LogContext.PushProperty("ClientIp", context.Connection.RemoteIpAddress);
            LogContext.PushProperty("Request", request);
            LogContext.PushProperty("Response", response);
            LogContext.PushProperty("Elapsed", elapsed);

            Serilog.Log.Information($"{context.Request.Method} - {context.Request.Path} - {context.Response.StatusCode} - in - {elapsed}ms");
        }

        double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
        private async Task<string> GetResponseAsTextAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            //Create stream reader to write entire stream
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }

        private async Task<string> GetRequestAsTextAsync(HttpRequest request)
        {
            // Ensure the request's body can be read multiple times 
            // (for the next middlewares in the pipeline).
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            // Reset the request's body stream position for 
            // next middleware in the pipeline.
            request.Body.Position = 0;
            return requestBody;
        }
    }
}