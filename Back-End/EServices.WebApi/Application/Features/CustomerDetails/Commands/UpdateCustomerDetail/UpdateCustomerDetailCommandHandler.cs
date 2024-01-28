
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.CustomerDetails.Commands.UpdateCustomerDetail
{
    public class UpdateCustomerDetailCommandHandler : IRequestHandler<UpdateCustomerDetailCommand, Response<int>>
    {
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepository;
        public UpdateCustomerDetailCommandHandler(ICustomerDetailRepositoryAsync CustomerDetailRepository)
        {
            _customerDetailRepository = CustomerDetailRepository;
        }
        public async Task<Response<int>> Handle(UpdateCustomerDetailCommand command, CancellationToken cancellationToken)
        {
            var CustomerDetail = await _customerDetailRepository.GetByIdAsync(command.Id);

            if (CustomerDetail == null)
            {
                throw new ApiException($"CustomerDetail Not Found.");
            }
            else
            {
                CustomerDetail.Client = command.Client;
                CustomerDetail.Plant = command.plant;
                CustomerDetail.AreaOfTest = command.AreaOfTest;
                CustomerDetail.EquipmentId = command.EquipmentId;
                CustomerDetail.InstrumentId = command.InstrumentId;
                CustomerDetail.DateOfTest = command.DateOfTest;
                CustomerDetail.FormType = command.FormType;
                CustomerDetail.DateOfTestDue=command.DateOfTestDue;
                CustomerDetail.TestCondition=command.TestCondition;
                CustomerDetail.ClassType = command.ClassType;

                await _customerDetailRepository.UpdateAsync(CustomerDetail);
                return new Response<int>(CustomerDetail.Id);
            }
        }

    }
}