using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Trainees.Commands.CreateTrainee
{
    public class CreateTraineeCommandHandler : IRequestHandler<CreateTraineeCommand, Response<int>>
    {
        private readonly ITraineeRepositoryAsync _traineeRepository;
        private readonly IMapper _mapper;
        public CreateTraineeCommandHandler(ITraineeRepositoryAsync TraineeRepository, IMapper mapper)
        {
            _traineeRepository = TraineeRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTraineeCommand request, CancellationToken cancellationToken)
        {
            var trainee = _mapper.Map<Domain.Entities.Trainee>(request);
            Expression<System.Func<Domain.Entities.Trainee, Domain.Entities.Trainee>> selectExpression = e => new()
            {
                Id = e.Id,
                Name = e.Name,
                EmployeeId = e.EmployeeId,
            };

            var traineePagedResponse = await _traineeRepository.GetPagedReponseAsync(0, 1, $"employeeId:eq:{request.EmployeeId}", null, selectExpression);

            if (traineePagedResponse.pagedResponse.Count > 0)
            {
                trainee.Id = traineePagedResponse.pagedResponse.FirstOrDefault().Id;
                await _traineeRepository.UpdateAsync(trainee);
            }
            else await _traineeRepository.AddAsync(trainee);

            return new Response<int>(trainee.Id);
        }

    }
}