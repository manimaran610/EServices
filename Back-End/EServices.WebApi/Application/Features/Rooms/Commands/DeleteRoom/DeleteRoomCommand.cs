
using Application.Wrappers;
using MediatR;

namespace Application.Features.Rooms.Commands.DeleteRoom
{
    public class DeleteRoomCommand: IRequest<Response<int>>
    {
        public int Id { get; set; }
        
    }
}