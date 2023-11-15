

using System.Collections.Generic;
using Application.Features.Rooms.Seeds;
using MediatR;

namespace Application.Features.DomainEvents.RoomGrillsAddRangeEvent
{
    public class RoomGrillsUpsertRangeEvent : INotification
    {
        public  int RoomId { get; set; }
        public List<GrillDto> Grills {get;set;}
    }
}