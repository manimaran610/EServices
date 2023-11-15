
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.DomainEvents.RoomGrillsAddRangeEvent;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Rooms.Commands.UpdateRoom
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Response<int>>
    {
        private readonly IRoomRepositoryAsync _roomRepository;
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateRoomCommandHandler
        (IRoomRepositoryAsync roomRepository,
        ICustomerDetailRepositoryAsync customerDetailRepositoryAsync,
        IRoomGrillRepositoryAsync roomGrillRepositoryAsync,
        IMapper mapper, 
        IMediator mediator
        )
        {
            _roomRepository = roomRepository;
            _customerDetailRepositoryAsync = customerDetailRepositoryAsync;
            _mapper = mapper;
            _mediator = mediator;

        }
        public async Task<Response<int>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var customerDetail = await _customerDetailRepositoryAsync.GetByIdAsync(request.CustomerDetailId);
            if (customerDetail == null)
                throw new ApiException($"CustomerDetail does not exists with CustomerDetailId -{request.CustomerDetailId}");

            var room = await _roomRepository.GetByIdAsync(request.Id);
            if (room == null)
            {
                throw new ApiException($"Room Not Found.");
            }
            else
            {
                room.Name = request.Name;
                room.NoOfGrills = request.NoOfGrills;
                room.AirChangesPerHour = request.AirChangesPerHour;
                room.DesignACPH = request.DesignACPH;
                room.TotalAirFlowCFM = request.TotalAirFlowCFM;
                room.RoomVolume = request.RoomVolume;
                room.CustomerDetailId = request.CustomerDetailId;

                await _roomRepository.UpdateAsync(room);


                if (customerDetail.FormType == FormType.ACPH)
                {
                    var roomGrillsUpsertRangeEvent = new RoomGrillsUpsertRangeEvent()
                    {
                        RoomId = room.Id,
                        Grills = request.RoomGrills
                    };
                    await _mediator.Publish(roomGrillsUpsertRangeEvent);
                }
                return new Response<int>(room.Id, $"Room updated along with Grills");
            }
        }

    }
}