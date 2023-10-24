using System;
using System.Collections.Generic;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Instruments.Queries.GetAllInstruments
{
    public class GetAllInstrumentsQuery : IRequest<PagedResponse<IEnumerable<GetAllInstrumentsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}