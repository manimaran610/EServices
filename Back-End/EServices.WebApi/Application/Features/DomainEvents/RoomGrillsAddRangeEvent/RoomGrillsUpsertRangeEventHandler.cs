using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.DomainEvents.RoomGrillsAddRangeEvent
{
    public class RoomGrillsUpsertRangeEventHandler : INotificationHandler<RoomGrillsUpsertRangeEvent>
    {
        private readonly IRoomGrillRepositoryAsync _roomGrillRepositoryAsync;
        private readonly IMapper _mapper;

        public RoomGrillsUpsertRangeEventHandler(IRoomGrillRepositoryAsync roomGrillRepositoryAsync, IMapper mapper)
        {
            _roomGrillRepositoryAsync = roomGrillRepositoryAsync;
            _mapper = mapper;
        }

        public async Task Handle(RoomGrillsUpsertRangeEvent request, System.Threading.CancellationToken cancellationToken)
        {

            var existingRoomGrills = await _roomGrillRepositoryAsync.GetPagedReponseAsync(0, int.MaxValue, $"RoomId:eq:{request.RoomId}");

            foreach (var grill in request.Grills)
            {
                var actualRoomGrill = existingRoomGrills.FirstOrDefault(e => e.ReferenceNumber == grill.ReferenceNumber);
                var roomGrill = _mapper.Map<RoomGrill>(grill);
                roomGrill.RoomId = request.RoomId;

                if (actualRoomGrill?.Id > 0)
                {
                    roomGrill.Id = actualRoomGrill.Id;
                    roomGrill.CreatedBy = actualRoomGrill.CreatedBy;
                    roomGrill.Created = actualRoomGrill.Created;
                }
                await _roomGrillRepositoryAsync.UpdateAsync(roomGrill);
            }

            //Soft Delete existing grills 
            foreach (var existingGrill in existingRoomGrills)
            {
                if (!request.Grills.Any(e => e.ReferenceNumber == existingGrill.ReferenceNumber))
                {
                    _roomGrillRepositoryAsync.SoftDeleteAsync(existingGrill);
                }
            }
        }


    }
}
