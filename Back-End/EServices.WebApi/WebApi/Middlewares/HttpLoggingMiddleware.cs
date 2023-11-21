﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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

        public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
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
            LogRequestResponse(context, elapsed,request,response);
        }


        private void LogRequestResponse(HttpContext context, double elapsed, string request, string response)
        {
            LogContext.PushProperty("QueryString", context.Request.QueryString);
            LogContext.PushProperty("StatusCode", context.Response.StatusCode);
            LogContext.PushProperty("Request", request);
            LogContext.PushProperty("Response", response);
            LogContext.PushProperty("Elapsed", elapsed);

            Serilog.Log.Information($"{context.Request.Method} - {context.Request.Path} - {context.Response.StatusCode}");
        }

        double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
        private async Task<string> GetRequestAsTextAsync(HttpRequest request)
        {
            var body = request.Body;

            //Set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            //Read request stream
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //Copy into  buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //Convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //Assign the read body back to the request body
            request.Body = body;
            // request.Body.Seek(0, SeekOrigin.Begin);

            return $"{bodyAsText}";
        }

        private async Task<string> GetResponseAsTextAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            //Create stream reader to write entire stream
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }
}