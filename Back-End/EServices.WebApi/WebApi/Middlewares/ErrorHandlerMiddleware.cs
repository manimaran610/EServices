using Application.Exceptions;
using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var start = Stopwatch.GetTimestamp();
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

                switch (error)
                {
                    case Application.Exceptions.ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        LogContext.PushProperty("Exception", e);
                        Serilog.Log.Warning(e.Message);
                        break;
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        LogContext.PushProperty("Exception", e);
                        Serilog.Log.Warning(e.Message);
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        LogContext.PushProperty("Exception", e);
                        Serilog.Log.Warning(e.Message);
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        LogContext.PushProperty("Exception", error);
                        Serilog.Log.Error(error.Message);
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
            var elapsed = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
            LogRequestResponse(context, elapsed);
        }
        private void LogRequestResponse(HttpContext context, double elapsed)
        {
            LogContext.PushProperty("QueryString", context.Request.QueryString);
            LogContext.PushProperty("StatusCode", context.Response.StatusCode);
            LogContext.PushProperty("Elapsed", elapsed);

            Serilog.Log.Information($"{context.Request.Method} - {context.Request.Path} - {context.Response.StatusCode}");
        }

        double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }



    }
}
