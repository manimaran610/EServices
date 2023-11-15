using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class CustomerDetailRepositoryAsync : GenericRepositoryAsync<CustomerDetail>, ICustomerDetailRepositoryAsync
    {

        public CustomerDetailRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}