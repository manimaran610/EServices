using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Identity.Contexts;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class RoomGrillRepositoryAsync : GenericRepositoryAsync<RoomGrill>, IRoomGrillRepositoryAsync
    {
        private readonly DbSet<RoomGrill> _roomGrills;


        public RoomGrillRepositoryAsync(
            ApplicationDbContext dbContext,
            IdentityContext identityContext,
            IAuthenticatedUserService authenticatedUserService
        ) :  base(dbContext, identityContext, authenticatedUserService)
        {
            _roomGrills = dbContext.Set<RoomGrill>();

        }
        public async Task<RoomGrill> GetByRoomIdAndReferenceNo(int roomId, string referenceNumber)
        {
            return await _roomGrills.FirstOrDefaultAsync(e => !e.IsDeleted && e.RoomId == roomId && e.ReferenceNumber == referenceNumber);
        }

    }
}