using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.CustomerDetails.Commands.DeleteCustomerDetail
{
    public class DeleteCustomerDetailCommandHandler : IRequestHandler<DeleteCustomerDetailCommand, Response<int>>
    {
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepository;
        public DeleteCustomerDetailCommandHandler(ICustomerDetailRepositoryAsync CustomerDetailRepository)
        {
            _customerDetailRepository = CustomerDetailRepository;
        }
        public async Task<Response<int>> Handle(DeleteCustomerDetailCommand command, CancellationToken cancellationToken)
        {
            var customerDetail = await _customerDetailRepository.GetByIdAsync(command.Id);
            if (customerDetail == null) throw new ApiException($"CustomerDetail Not Found.");
            await _customerDetailRepository.SoftDeleteAsync(customerDetail);
            return new Response<int>(customerDetail.Id);
        }
    }
}

