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
using Microsoft.EntityFrameworkCore.DynamicLinq;

namespace Infrastructure.Persistence.Repository
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : AuditableBaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id, Expression<System.Func<T, T>> selectExpression = null)
        {
            selectExpression = selectExpression == null ? e => e : selectExpression;
            return await _dbContext.Set<T>()
            .Where(e => e.Id == id && !e.IsDeleted)
            .Select(selectExpression)
            .FirstOrDefaultAsync();

        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync
        (
            int offset,
            int count,
            string filter = null,
            string sort = null,
            Expression<System.Func<T, T>> selectExpression = null,
            string filterOperator = null
        )
        {
            selectExpression = selectExpression == null ? e => e : selectExpression;
            sort = sort == null ? "Created:desc" : sort + ",Created:desc";

            return await _dbContext
                .Set<T>()
                .Where(e => !e.IsDeleted)
                .GetFilteredList(filter, filterOperator)
                .GetSortedList(sort)
                .Select(selectExpression)
                .AsNoTracking()
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
        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            return entities;
        }

        public async Task UpdateAsync(T entity)
        {
            // _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Update<T>(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task SoftDeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            await UpdateAsync(entity);
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
         .Set<T>()
         .Count(e => !e.IsDeleted));
        }

    }
}
