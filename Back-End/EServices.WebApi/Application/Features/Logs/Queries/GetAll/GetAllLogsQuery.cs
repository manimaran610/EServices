using System;
using System.Collections.Generic;
using Application.Parameters;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Instruments.Queries.GetAllInstruments
{
    public class GetAllLogsQuery :IRequestParameter, IRequest<PagedResponse<IEnumerable<Log>>>
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }

    }

}