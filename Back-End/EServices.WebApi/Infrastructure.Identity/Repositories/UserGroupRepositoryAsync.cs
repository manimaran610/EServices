using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity.Contexts;

namespace Infrastructure.Persistence.Repositories
{
    public class UserGroupRepositoryAsync : GenericRepositoryAsync<UserGroup>, IUserGroupRepositoryAsync
    {
        private readonly DbSet<Group> _groups;

        public UserGroupRepositoryAsync(IdentityContext dbContext) : base(dbContext)
        {
            _groups = dbContext.Set<Group>();
        }

     
    }
}
