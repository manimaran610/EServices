
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Instruments.Commands.UpdateInstrument
{
    public class UpdateInstrumentCommandHandler : IRequestHandler<UpdateInstrumentCommand, Response<int>>
    {
        private readonly IInstrumentRepositoryAsync _instrumentRepository;
        public UpdateInstrumentCommandHandler(IInstrumentRepositoryAsync InstrumentRepository)
        {
            _instrumentRepository = InstrumentRepository;
        }
        public async Task<Response<int>> Handle(UpdateInstrumentCommand command, CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository.GetByIdAsync(command.Id);

            if (instrument == null)
            {
                throw new ApiException($"Instrument Not Found.");
            }
            else
            {
                instrument.SerialNumber = command.SerialNumber;
                instrument.Make = command.Make;
                instrument.Model = command.Model;
                instrument.CalibratedOn = command.CalibratedOn;
                instrument.CalibratedDueOn = command.CalibratedDueOn;
                instrument.CertificateName =command.CertificateName;
              
                await _instrumentRepository.UpdateAsync(instrument);
                return new Response<int>(instrument.Id);
            }
        }
        private async Task<byte[]> ReadFileContent(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}