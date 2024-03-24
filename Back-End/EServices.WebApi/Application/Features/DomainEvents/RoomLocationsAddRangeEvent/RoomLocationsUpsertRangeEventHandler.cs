using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Rooms.Seeds;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.DomainEvents.RoomLocationsAddRangeEvent
{
    public class RoomLocationsUpsertRangeEventHandler : INotificationHandler<RoomLocationsUpsertRangeEvent>
    {
        private readonly IRoomLocationRepositoryAsync _roomLocationRepositoryAsync;
        private readonly IMapper _mapper;

        public RoomLocationsUpsertRangeEventHandler(IRoomLocationRepositoryAsync roomLocationRepositoryAsync, IMapper mapper)
        {
            _roomLocationRepositoryAsync = roomLocationRepositoryAsync;
            _mapper = mapper;
        }

        public async Task Handle(RoomLocationsUpsertRangeEvent request, System.Threading.CancellationToken cancellationToken)
        {

            var existingRoomLocations = await _roomLocationRepositoryAsync.GetPagedReponseAsync(0, int.MaxValue, $"RoomId:eq:{request.RoomId}");

            foreach (var Location in request.Locations)
            {
                var actualRoomLocation = existingRoomLocations.pagedResponse.FirstOrDefault(e => e.ReferenceNumber == Location.ReferenceNumber);
                var roomLocation = _mapper.Map<RoomLocation>(Location);
                roomLocation.RoomId = request.RoomId;

                if (actualRoomLocation?.Id > 0)
                {
                    roomLocation.Id = actualRoomLocation.Id;
                    roomLocation.CreatedBy = actualRoomLocation.CreatedBy;
                    roomLocation.Created = actualRoomLocation.Created;
                }
                await _roomLocationRepositoryAsync.UpdateAsync(roomLocation);
            }

            await SoftDeleteExisitingRecords(existingRoomLocations.pagedResponse, request.Locations);


        }

        private async Task SoftDeleteExisitingRecords(IReadOnlyList<RoomLocation> roomLocations, IList<LocationDTO> requestLocations)
        {
            //Soft Delete existing Locations 
            foreach (var existingLocation in roomLocations)
            {
                if (!requestLocations.Any(e => e.ReferenceNumber == existingLocation.ReferenceNumber))
                {
                    await _roomLocationRepositoryAsync.SoftDeleteAsync(existingLocation);
                }
            }
        }
    }
}
