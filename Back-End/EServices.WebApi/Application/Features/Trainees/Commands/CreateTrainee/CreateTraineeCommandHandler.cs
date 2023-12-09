using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var Trainee = _mapper.Map<Domain.Entities.Trainee>(request);
                     
            await _traineeRepository.AddAsync(Trainee);
            return new Response<int>(Trainee.Id);
        }

    }
}