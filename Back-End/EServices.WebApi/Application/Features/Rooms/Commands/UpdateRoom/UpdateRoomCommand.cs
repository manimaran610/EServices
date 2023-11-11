

using System;
using System.Collections.Generic;
using Application.Features.Rooms.Seeds;
using Application.Wrappers;
using Domain.Enums;
using MediatR;

namespace Application.Features.Rooms.Commands.UpdateRoom
{
    public class UpdateRoomCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DesignACPH { get; set; }
        public int NoOfGrills { get; set; }
        public int RoomVolume { get; set; }
        public int TotalAirFlowCFM { get; set; }
        public int AirChangesPerHour { get; set; }
        public int CustomerDetailId { get; set; }

        public List<GrillDto> Grills { get; set; } = new();
    }
}