using System;
using System.Collections.Generic;
using Application.Parameters;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Trainees.Queries.GetAllTrainees
{
    public class GetAllTraineesQuery :IRequestParameter, IRequest<PagedResponse<IEnumerable<GetAllTraineesViewModel>>>
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }

    }

}