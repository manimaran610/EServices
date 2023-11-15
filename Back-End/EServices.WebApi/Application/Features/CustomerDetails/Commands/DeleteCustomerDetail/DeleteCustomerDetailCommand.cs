
using Application.Wrappers;
using MediatR;

namespace Application.Features.CustomerDetails.Commands.DeleteCustomerDetail
{
    public class DeleteCustomerDetailCommand: IRequest<Response<int>>
    {
        public int Id { get; set; }
        
    }
}