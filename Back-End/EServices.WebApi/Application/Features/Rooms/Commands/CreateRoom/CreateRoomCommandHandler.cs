
using System.Threading;
using System.Threading.Tasks;
using Application.Features.DomainEvents;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Application.Features.DomainEvents.RoomGrillsAddRangeEvent;
using Application.Features.DomainEvents.RoomLocationsAddRangeEvent;

namespace Application.Features.Rooms.Commands.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Response<int>>
    {
        private readonly IRoomRepositoryAsync _roomRepository;
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepositoryAsync;
        private readonly IRoomGrillRepositoryAsync _roomGrillRepositoryAsync;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CreateRoomCommandHandler
        (
            IRoomRepositoryAsync RoomRepository,
            ICustomerDetailRepositoryAsync customerDetailRepositoryAsync,
            IRoomGrillRepositoryAsync roomGrillRepositoryAsync,
            IMapper mapper,
            IMediator mediator
        )
        {
            _roomRepository = RoomRepository;
            _customerDetailRepositoryAsync = customerDetailRepositoryAsync;
            _roomGrillRepositoryAsync = roomGrillRepositoryAsync;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response<int>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            var room = _mapper.Map<Domain.Entities.Room>(request);
            var customerDetail = await _customerDetailRepositoryAsync.GetByIdAsync(request.CustomerDetailId);
            if (customerDetail == null)
                throw new ApiException($"CustomerDetail does not exists with CustomerDetailId -{request.CustomerDetailId}");

            await _roomRepository.AddAsync(room);

            if (customerDetail.FormType == FormType.ACPH)
            {
                var domainEvent = new RoomGrillsUpsertRangeEvent()
                {
                    RoomId = room.Id,
                    Grills = request.RoomGrills
                };
                await _mediator.Publish(domainEvent);
                message = $"Room added along with Grills";
            }
            else if (customerDetail.FormType == FormType.ParticleCountThreeCycle)
            {
                var domainEvent = new RoomLocationsUpsertRangeEvent()
                {
                    RoomId = room.Id,
                    Locations = request.RoomLocations
                };
                await _mediator.Publish(domainEvent);
                message = $"Room added along with Locations";

            }
            return new Response<int>(room.Id, message);
        }


    }
}