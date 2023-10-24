using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            instrument.CertificateName = request.Certificate.FileName;
            instrument.CertificateFile = await ReadFileContent(request.Certificate);
            
            await _instrumentRepository.AddAsync(instrument);
            return new Response<int>(instrument.Id);
        }

        private async Task<byte[]> ReadFileContent(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}