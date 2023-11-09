using System;
using System.Collections.Generic;
using Application.Parameters;
using Application.Wrappers;
using MediatR;

namespace Application.Features.CustomerDetails.Queries.GetAllCustomerDetails
{
    public class GetAllCustomerDetailsQuery :IRequestParameter, IRequest<PagedResponse<IEnumerable<GetAllCustomerDetailsViewModel>>>
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }

    }

}