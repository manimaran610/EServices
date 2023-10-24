

using System;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Instruments.Commands.UpdateInstrument
{
    public class UpdateInstrumentCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime CalibratedOn { get; set; }
        public DateTime CalibratedDueOn { get; set; }
        public IFormFile Certificate { get; set; } =null!;
        public string CertificateName { get; set; }
        public byte[] CertificateFile { get; set; } = default;
    }
}