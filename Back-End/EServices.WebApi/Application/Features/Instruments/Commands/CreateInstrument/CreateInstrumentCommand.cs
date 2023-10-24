using System;
using System.Text.Json.Serialization;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Instruments.Commands.CreateInstrument
{
    public class CreateInstrumentCommand : IRequest<Response<int>>
    {
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime CalibratedOn { get; set; }
        public DateTime CalibratedDueOn { get; set; }
        public IFormFile Certificate { get; set; }
        public string CertificateName { get; set; }
        public byte[] CertificateFile { get; set; } =default!;
    }
}