
using System;

namespace Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string MachineName { get; set; }
        public string ClientIp { get; set; }
        public int? StatusCode { get; set; }
        public string QueryString { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Exception { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Elapsed { get; set; }

    }
}