using System;
using System.Collections.Generic;
using Application.Parameters;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Rooms.Queries.GetAllRooms
{
    public class GetAllRoomsQuery :IRequestParameter, IRequest<PagedResponse<IEnumerable<GetAllRoomsViewModel>>>
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }

    }

}