


using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomGrillRepositoryAsync : IGenericRepositoryAsync<RoomGrill>
    {
        Task<RoomGrill> GetByRoomIdAndReferenceNo(int roomId, string referenceNumber);

    }
}