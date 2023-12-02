using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Persistence.Extensions;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Persistence.Repositories
{

    public class LogRepositoryAsync : ILogRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public LogRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<Log>> GetPagedReponseAsync
        (
            int offset,
            int count,
            string filter = null,
            string sort = null,
            string filterOperator = null
        )
        {
            sort = sort == null ? "Id:desc" : sort + ",Id:desc";

            return await _dbContext
                .Set<Log>()
                .GetFilteredList<Log>(filter, filterOperator)
                .GetSortedList(sort)
                .AsNoTracking()
                .Skip(offset)
                .Take(count)
                .ToDynamicListAsync<Log>();
        }

        public async Task<int> TotalCountAsync()
        {
            return await Task.FromResult(_dbContext
         .Set<Log>()
         .Count());
        }
    }
}