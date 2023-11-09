

using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.CustomerDetails.Queries.GetCustomerDetailById
{
    public class GetCustomerDetailByIdQueryHandler : IRequestHandler<GetCustomerDetailByIdQuery, Response<CustomerDetail>>
    {
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepository;
        public GetCustomerDetailByIdQueryHandler(ICustomerDetailRepositoryAsync CustomerDetailRepository)
        {
            _customerDetailRepository = CustomerDetailRepository;
        }
        public async Task<Response<CustomerDetail>> Handle(GetCustomerDetailByIdQuery query, CancellationToken cancellationToken)
        {
            var customerDetail = await _customerDetailRepository.GetByIdAsync(query.Id);
            if (customerDetail == null) throw new ApiException($"CustomerDetail Not Found.");
            return new Response<CustomerDetail>(customerDetail);
        }
    }
}