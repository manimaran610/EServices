

using System;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Features.Instruments.Commands.UpdateInstrument
{
    public class UpdateInstrumentCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
         [JsonProperty("serialNo")]
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime CalibratedOn { get; set; }
        public DateTime CalibratedDueOn { get; set; }
        public string CertificateName { get; set; }
        public string CertificateFile { get; set; } = default;
    }
}