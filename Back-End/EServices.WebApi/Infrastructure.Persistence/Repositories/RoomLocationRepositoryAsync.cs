using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Identity.Contexts;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;

namespace Infrastructure.Persistence.Repositories
{

    public class RoomLocationRepositoryAsync : GenericRepositoryAsync<RoomLocation>, IRoomLocationRepositoryAsync
    {

        public RoomLocationRepositoryAsync (
            ApplicationDbContext dbContext,
            IdentityContext identityContext,
            IAuthenticatedUserService authenticatedUserService
        ) :  base(dbContext, identityContext, authenticatedUserService){}

    }
}