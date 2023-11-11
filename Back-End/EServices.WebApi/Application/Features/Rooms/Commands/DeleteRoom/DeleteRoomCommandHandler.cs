using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Rooms.Commands.DeleteRoom
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Response<int>>
    {
        private readonly IRoomRepositoryAsync _roomRepository;
        public DeleteRoomCommandHandler(IRoomRepositoryAsync RoomRepository)
        {
            _roomRepository = RoomRepository;
        }
        public async Task<Response<int>> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
        {
            var Room = await _roomRepository.GetByIdAsync(command.Id);
            if (Room == null) throw new ApiException($"Room Not Found.");
            await _roomRepository.SoftDeleteAsync(Room);
            return new Response<int>(Room.Id);
        }
    }
}

