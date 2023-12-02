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
    public class GetAllLogsQueryHandler : IRequestHandler<GetAllLogsQuery, PagedResponse<IEnumerable<Log>>>
    {
        private readonly ILogRepositoryAsync _logRepository;
        private readonly IMapper _mapper;
        public GetAllLogsQueryHandler(ILogRepositoryAsync logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<Log>>> Handle(GetAllLogsQuery request, CancellationToken cancellationToken)
        {
            var reqParam = _mapper.Map<RequestParameter>(request);

            var pagedResponse = await _logRepository.GetPagedReponseAsync(reqParam.Offset, reqParam.Count, reqParam.Filter, reqParam.Sort);
            var totalCount = await _logRepository.TotalCountAsync();

            var instrumentViewModel = _mapper.Map<IEnumerable<Log>>(pagedResponse);
            return new PagedResponse<IEnumerable<Log>>(instrumentViewModel, request.Offset, request.Count, instrumentViewModel.Count(), totalCount);
        }
    }
}