
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace WebApi.Extensions
{

    public static class ExceptionExtensions
    {
        public class ExceptionInfo
        {
            public ExceptionInfo() { }

            internal ExceptionInfo(Exception exception, bool includeInnerException = true, bool includeStackTrace = false)
            {
                if (exception is null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }

                Type = exception.GetType().FullName;
                Message = exception.Message;
                Source = exception.Source;
                StackTrace = includeStackTrace ? exception.StackTrace : null;
                if (includeInnerException && exception.InnerException is not null)
                {
                    InnerException = new ExceptionInfo(exception.InnerException, includeInnerException, includeStackTrace);
                }
            }

            public string Type { get; set; }
            public string Message { get; set; }
            public string Source { get; set; }
            public string StackTrace { get; set; }
            public ExceptionInfo InnerException { get; set; }
        }

        private static readonly JsonSerializerOptions _defaultJsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        /// <summary>
        /// Serialize the <see cref="Exception"/> to a JSON string.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="includeInnerException">Control if to include inner exception</param>
        /// <param name="includeStackTrace">Control if to include stack trace</param>
        /// <param name="options">JSON options. By default nulls are not serialized and the string is indented</param>
        /// <returns></returns>
        public static string ToJson(
            this Exception ex,
            bool includeInnerException = true,
            bool includeStackTrace = false,
            JsonSerializerOptions options = null)
        {
            ArgumentNullException.ThrowIfNull(ex);
            var info = new ExceptionInfo(ex, includeInnerException, includeStackTrace);

            return JsonSerializer.Serialize(info, options ?? _defaultJsonSerializerOptions);
        }
    }
}