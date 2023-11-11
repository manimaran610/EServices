
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Rooms.Queries.GetRoomById
{
    public class GetRoomByIdQuery : IRequest<Response<Room>>
    {
        public int Id { get; set; }
    }
}