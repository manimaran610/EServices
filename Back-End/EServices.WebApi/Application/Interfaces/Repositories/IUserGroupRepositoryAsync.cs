


using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUserGroupRepositoryAsync:IGenericRepositoryAsync<UserGroup>
    {
        Task<bool> CheckUserAccessInSameGroup(string authUserId, string userId);
    }
}