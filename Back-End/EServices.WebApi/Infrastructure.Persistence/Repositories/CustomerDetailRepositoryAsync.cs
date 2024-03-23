using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Identity.Contexts;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class CustomerDetailRepositoryAsync : GenericRepositoryAsync<CustomerDetail>, ICustomerDetailRepositoryAsync
    {

        public CustomerDetailRepositoryAsync
        (
            ApplicationDbContext dbContext,
            IdentityContext identityContext,
            IAuthenticatedUserService authenticatedUserService
        ) :  base(dbContext, identityContext, authenticatedUserService){}

    }
}