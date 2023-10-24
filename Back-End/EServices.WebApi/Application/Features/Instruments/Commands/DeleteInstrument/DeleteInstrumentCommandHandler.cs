using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Instruments.Commands.DeleteInstrument
{
    public class DeleteInstrumentCommandHandler : IRequestHandler<DeleteInstrumentCommand, Response<int>>
    {
        private readonly IInstrumentRepositoryAsync _instrumentRepository;
        public DeleteInstrumentCommandHandler(IInstrumentRepositoryAsync instrumentRepository)
        {
            _instrumentRepository = instrumentRepository;
        }
        public async Task<Response<int>> Handle(DeleteInstrumentCommand command, CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository.GetByIdAsync(command.Id);
            if (instrument == null) throw new ApiException($"instrument Not Found.");
            await _instrumentRepository.DeleteAsync(instrument);
            return new Response<int>(instrument.Id);
        }
    }
}

