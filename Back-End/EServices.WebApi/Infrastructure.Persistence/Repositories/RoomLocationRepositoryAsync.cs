using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;

namespace Infrastructure.Persistence.Repositories
{

    public class RoomLocationRepositoryAsync : GenericRepositoryAsync<RoomLocation>, IRoomLocationRepositoryAsync
    {

        public RoomLocationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext) {}

    }
}