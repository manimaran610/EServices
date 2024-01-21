

using System.Collections.Generic;
using Application.Features.Rooms.Seeds;
using MediatR;

namespace Application.Features.DomainEvents.RoomLocationsAddRangeEvent
{
    public class RoomLocationsUpsertRangeEvent : INotification
    {
        public  int RoomId { get; set; }
        public List<LocationDTO> Locations {get;set;}
    }
}