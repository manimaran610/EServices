using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviours.DomainEvents.RoomGrillsAddRangeEvent
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

            foreach (var grill in request.Grills)
            {
                var actualRoomGrill = await _roomGrillRepositoryAsync.GetByRoomIdAndReferenceNo(request.RoomId, grill.ReferenceNumber);
                var roomGrill = _mapper.Map<RoomGrill>(grill);
                roomGrill.RoomId = request.RoomId;

                if (actualRoomGrill?.Id > 0) roomGrill.Id = actualRoomGrill.Id;
                await _roomGrillRepositoryAsync.UpdateAsync(roomGrill);         
            }
        }


    }
}
