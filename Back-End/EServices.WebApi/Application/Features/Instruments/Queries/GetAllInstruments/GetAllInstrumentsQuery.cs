using System;
using System.Collections.Generic;
using Application.Parameters;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Instruments.Queries.GetAllInstruments
{
    public class GetAllInstrumentsQuery :IRequestParameter, IRequest<PagedResponse<IEnumerable<GetAllInstrumentsViewModel>>>
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }

    }

}