using System;
using System.Collections.Generic;
using Application.Features.Rooms.Seeds;
using Domain.Enums;

namespace Application.Features.Rooms.Queries.GetAllRooms
{
    public class GetAllRoomsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DesignACPH { get; set; }
        public string ClassType { get; set; }
        public int NoOfGrills { get; set; }
        public int NoOfLocations { get; set; }
        public int AreaM2 { get; set; }
        public int RoomVolume { get; set; }
        public int TotalAirFlowCFM { get; set; }
        public int AirChangesPerHour { get; set; }
        public int CustomerDetailId { get; set; }

        public List<GrillDTO> RoomGrills { get; set; } = new();
        public List<LocationDTO> RoomLocations { get; set; } = new();

    }
}