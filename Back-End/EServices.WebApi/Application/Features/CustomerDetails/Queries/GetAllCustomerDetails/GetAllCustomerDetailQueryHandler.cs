using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.CustomerDetails.Queries.GetAllCustomerDetails
{
    public class GetAllCustomerDetailsQueryHandler : IRequestHandler<GetAllCustomerDetailsQuery, PagedResponse<IEnumerable<GetAllCustomerDetailsViewModel>>>
    {
        private readonly ICustomerDetailRepositoryAsync _xustomerDetailRepository;
        private readonly IMapper _mapper;
        public GetAllCustomerDetailsQueryHandler(ICustomerDetailRepositoryAsync CustomerDetailRepository, IMapper mapper)
        {
            _xustomerDetailRepository = CustomerDetailRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllCustomerDetailsViewModel>>> Handle(GetAllCustomerDetailsQuery request, CancellationToken cancellationToken)
        {

            var reqParam = _mapper.Map<RequestParameter>(request);

            var CustomerDetail = await _xustomerDetailRepository.GetPagedReponseAsync(reqParam.Offset, reqParam.Count, reqParam.Filter, reqParam.Sort);
            var totalCount =  CustomerDetail.totalCount;

            var CustomerDetailViewModel = _mapper.Map<IEnumerable<GetAllCustomerDetailsViewModel>>(CustomerDetail.pagedResponse);
            return new PagedResponse<IEnumerable<GetAllCustomerDetailsViewModel>>(CustomerDetailViewModel, request.Offset, request.Count, CustomerDetailViewModel.Count(), totalCount);
        }
    }
}