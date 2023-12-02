

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Rooms.Queries.GetRoomById
{
    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, Response<Room>>
    {
        private readonly IRoomRepositoryAsync _roomRepository;
        public GetRoomByIdQueryHandler(IRoomRepositoryAsync RoomRepository)
        {
            _roomRepository = RoomRepository;
        }
        public async Task<Response<Room>> Handle(GetRoomByIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<Room, Room>> selectExpression = e => new()
            {
                Id = e.Id,
                DesignACPH = e.DesignACPH,
                AirChangesPerHour = e.AirChangesPerHour,
                NoOfGrills = e.NoOfGrills,
                NoOfLocations = e.NoOfLocations,
                ClassType=e.ClassType,
                AreaM2 =e.AreaM2,
                RoomVolume = e.RoomVolume,
                CustomerDetailId = e.CustomerDetailId,
                Name = e.Name,
                TotalAirFlowCFM = e.TotalAirFlowCFM,
                RoomGrills = e.RoomGrills.Where(e => !e.IsDeleted).ToList(),
                RoomLocations = e.RoomLocations.Where(e => !e.IsDeleted).ToList()


            };
            var Room = await _roomRepository.GetByIdAsync(query.Id, selectExpression);
            if (Room == null) throw new ApiException($"Room Not Found.");
            return new Response<Room>(Room);
        }
    }
}