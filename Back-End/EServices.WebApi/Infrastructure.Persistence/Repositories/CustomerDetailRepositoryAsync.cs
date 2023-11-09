using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class CustomerDetailRepositoryAsync : GenericRepositoryAsync<CustomerDetail>, ICustomerDetailRepositoryAsync
    {
        private readonly DbSet<CustomerDetail> _customerDetailss;

        public CustomerDetailRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _customerDetailss = dbContext.Set<CustomerDetail>();
        }

    }
}