using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Instruments.Commands.CreateInstrument
{
    public class CreateInstrumentCommandHandler : IRequestHandler<CreateInstrumentCommand, Response<int>>
    {
        private readonly IInstrumentRepositoryAsync _instrumentRepository;
        private readonly IMapper _mapper;
        public CreateInstrumentCommandHandler(IInstrumentRepositoryAsync InstrumentRepository, IMapper mapper)
        {
            _instrumentRepository = InstrumentRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var instrument = _mapper.Map<Domain.Entities.Instrument>(request);
            var instruments = await _instrumentRepository.GetPagedReponseAsync(0, 1, $"Type:eq:{request.Type},SerialNumber:eq:{request.SerialNumber}");
           
            if (instruments.Any())
                throw new ApiException($"Instrument already exists with type {request.Type} and serial number {request.SerialNumber}");
                
            await _instrumentRepository.AddAsync(instrument);
            return new Response<int>(instrument.Id);
        }

    }
}