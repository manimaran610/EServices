using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class TraineeRepositoryAsync : GenericRepositoryAsync<Trainee>, ITraineeRepositoryAsync
    {
        public TraineeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}