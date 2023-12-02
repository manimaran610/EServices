


using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ILogRepositoryAsync
    {
        Task<int> TotalCountAsync();
        Task<IReadOnlyList<Log>> GetPagedReponseAsync(int offset, int count, string filter = null, string sort = null, string filterOperator = null);

    }
}