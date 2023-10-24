using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Instruments.Queries.GetAllInstruments
{
    public class GetAllInstrumentsQueryHandler: IRequestHandler<GetAllInstrumentsQuery, PagedResponse<IEnumerable<GetAllInstrumentsViewModel>>>
    {
        private readonly IInstrumentRepositoryAsync _instrumentRepository;
        private readonly IMapper _mapper;
        public GetAllInstrumentsQueryHandler(IInstrumentRepositoryAsync InstrumentRepository, IMapper mapper)
        {
            _instrumentRepository = InstrumentRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllInstrumentsViewModel>>> Handle(GetAllInstrumentsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllInstrumentsParameter>(request);
            var instrumentPagedResponse = await _instrumentRepository.GetPagedReponseAsync(validFilter.PageNumber,validFilter.PageSize);
            var instrumentViewModel = _mapper.Map<IEnumerable<GetAllInstrumentsViewModel>>(instrumentPagedResponse);
            return new PagedResponse<IEnumerable<GetAllInstrumentsViewModel>>(instrumentViewModel, validFilter.PageNumber, validFilter.PageSize);           
        }
    }
}