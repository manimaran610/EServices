
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CustomerDetails.Commands.CreateCustomerDetail
{
    public class CreateCustomerDetailCommandHandler : IRequestHandler<CreateCustomerDetailCommand, Response<int>>
    {
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepository;
        private readonly IMapper _mapper;
        public CreateCustomerDetailCommandHandler(ICustomerDetailRepositoryAsync CustomerDetailRepository, IMapper mapper)
        {
            _customerDetailRepository = CustomerDetailRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateCustomerDetailCommand request, CancellationToken cancellationToken)
        {
            var customerDetail = _mapper.Map<Domain.Entities.CustomerDetail>(request);
                     
            await _customerDetailRepository.AddAsync(customerDetail);
            return new Response<int>(customerDetail.Id);
        }

    }
}