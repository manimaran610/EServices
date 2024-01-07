using System;
using Application.Wrappers;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.Trainees.Commands.CreateTrainee
{
    public class CreateTraineeCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string EmployeeId { get; set; }
        public string CertificateName { get; set; }
        public string CertificateFile { get; set; } = default!;
    }
}