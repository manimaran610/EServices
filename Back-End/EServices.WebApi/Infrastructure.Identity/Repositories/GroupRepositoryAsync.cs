using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity.Contexts;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupRepositoryAsync : GenericRepositoryAsync<Group>, IGroupRepositoryAsync
    {
        private readonly DbSet<Group> _groups;

        public GroupRepositoryAsync(IdentityContext dbContext) : base(dbContext)
        {
            _groups = dbContext.Set<Group>();
        }

     
    }
}
