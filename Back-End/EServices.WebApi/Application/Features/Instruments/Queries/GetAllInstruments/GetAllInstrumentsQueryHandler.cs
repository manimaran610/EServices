using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Filters;
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

            var reqParam = _mapper.Map<RequestParameter>(request);
            Expression<System.Func<Instrument, Instrument>> selectExpression = e => new()
            {
                Id = e.Id,
                SerialNumber = e.SerialNumber,
                Make = e.Make,
                Model = e.Model,
                Type = e.Type,
                CalibratedOn = e.CalibratedOn,
                CalibratedDueOn = e.CalibratedDueOn,
                CertificateName=e.CertificateName
            };

            var instruments = await _instrumentRepository.GetPagedReponseAsync(reqParam.Offset, reqParam.Count, reqParam.Filter, reqParam.Sort, selectExpression);

            var instrumentViewModel = _mapper.Map<IEnumerable<GetAllInstrumentsViewModel>>(instruments.pagedResponse);
            return new PagedResponse<IEnumerable<GetAllInstrumentsViewModel>>(instrumentViewModel, request.Offset, request.Count, instrumentViewModel.Count(), instruments.totalCount);
        }
    }
}