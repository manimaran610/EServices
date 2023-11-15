using System;
using Application.Wrappers;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.Instruments.Commands.CreateInstrument
{
    public class CreateInstrumentCommand : IRequest<Response<int>>
    {
        [JsonProperty("serialNo")]
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public DateTime CalibratedOn { get; set; }
        public DateTime CalibratedDueOn { get; set; }
        public string CertificateName { get; set; }
        public string CertificateFile { get; set; } = default!;
    }
}