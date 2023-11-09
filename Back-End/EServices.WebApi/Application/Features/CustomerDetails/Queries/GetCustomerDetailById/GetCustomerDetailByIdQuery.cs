
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.CustomerDetails.Queries.GetCustomerDetailById
{
    public class GetCustomerDetailByIdQuery : IRequest<Response<CustomerDetail>>
    {
        public int Id { get; set; }
    }
}