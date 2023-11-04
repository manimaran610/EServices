using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Instruments.Queries.GetAllInstruments
{
    public class GetAllInstrumentsQueryHandler : IRequestHandler<GetAllInstrumentsQuery, PagedResponse<IEnumerable<GetAllInstrumentsViewModel>>>
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

            Expression<System.Func<Instrument, Instrument>> selectExpression = e => new()
            {
                Id = e.Id,
                SerialNumber = e.SerialNumber,
                Make = e.Make,
                Model = e.Model,
                Type=e.Type,
                CalibratedOn = e.CalibratedOn,
                CalibratedDueOn = e.CalibratedDueOn
            };
            var instrumentPagedResponse = await _instrumentRepository.GetPagedReponseAsync(request.Offset, request.Count, request.Filter, request.Sort, selectExpression);
            var totalCount = await _instrumentRepository.TotalCountAsync();

            var instrumentViewModel = _mapper.Map<IEnumerable<GetAllInstrumentsViewModel>>(instrumentPagedResponse);
            return new PagedResponse<IEnumerable<GetAllInstrumentsViewModel>>(instrumentViewModel, request.Offset, request.Count, instrumentViewModel.Count(), totalCount);
        }
    }
}