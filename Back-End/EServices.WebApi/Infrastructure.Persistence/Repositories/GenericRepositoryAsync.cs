using Application.Interfaces;
using Domain.Common;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repository
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : AuditableBaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync
        (
            int offset,
            int count,
            string filter = null,
            string sort = null,
            Expression<System.Func<T, T>> selectExpression = null
        )
        {
            selectExpression = selectExpression == null ? e => e : selectExpression;
            sort = sort == null ? "Created:desc" : sort + ",Created:desc";

            return await _dbContext
                .Set<T>()
                .GetFilteredList(filter)
                .GetSortedList(sort)
                .AsNoTracking()
                .Select(selectExpression)
                .Skip(offset)
                .Take(count)
                .ToDynamicListAsync<T>();

        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext
                 .Set<T>()
                 .ToListAsync();
        }

        public async Task<int> TotalCountAsync()
        {
            return await Task.FromResult(_dbContext
         .Set<T>().Count());
        }

    }
}
