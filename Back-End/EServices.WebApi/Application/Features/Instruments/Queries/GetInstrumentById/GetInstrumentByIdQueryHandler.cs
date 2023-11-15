

using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Instruments.Queries.GetInstrumentById
{
    public class GetInstrumentByIdQueryHandler : IRequestHandler<GetInstrumentByIdQuery, Response<Instrument>>
    {
        private readonly IInstrumentRepositoryAsync _instrumentRepository;
        public GetInstrumentByIdQueryHandler(IInstrumentRepositoryAsync InstrumentRepository)
        {
            _instrumentRepository = InstrumentRepository;
        }
        public async Task<Response<Instrument>> Handle(GetInstrumentByIdQuery query, CancellationToken cancellationToken)
        {
            var Instrument = await _instrumentRepository.GetByIdAsync(query.Id);
            if (Instrument == null) throw new ApiException($"Instrument Not Found.");
            return new Response<Instrument>(Instrument);
        }
    }
}