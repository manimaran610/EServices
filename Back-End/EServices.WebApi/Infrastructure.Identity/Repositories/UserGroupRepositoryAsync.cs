using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity.Contexts;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class UserGroupRepositoryAsync : GenericRepositoryAsync<UserGroup>, IUserGroupRepositoryAsync
    {

        private readonly IdentityContext _identityContext;
        public UserGroupRepositoryAsync(IdentityContext dbContext) : base(dbContext)
        {
            _identityContext = dbContext;
        }

        public async Task<bool> CheckUserAccessInSameGroup(string authUserId, string userId)
        {
            var users = await _identityContext.UserGroups
                .Join(_identityContext.UserGroups, e => e.GroupId, x => x.GroupId, (u, ug) => new { CurrentUserId = u.UserId, ug.UserId })
                .Where(ug => ug.CurrentUserId == authUserId)
                .Select(u => u.UserId).ToListAsync();
            return users.Any(e => e == userId);

        }

    }
}
