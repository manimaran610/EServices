using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class RoomRepositoryAsync : GenericRepositoryAsync<Room>, IRoomRepositoryAsync
    {

        public RoomRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}