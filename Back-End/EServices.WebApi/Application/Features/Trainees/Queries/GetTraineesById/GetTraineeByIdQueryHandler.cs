using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Trainees.Queries.GetTraineesById;

public class GetTraineeByIdQueryHandler : IRequestHandler<GetTraineeByIdQuery, Response<Trainee>>
    {  private readonly ITraineeRepositoryAsync _traineeRepository;
        public GetTraineeByIdQueryHandler(ITraineeRepositoryAsync TraineeRepository)
        {
            _traineeRepository = TraineeRepository;
        }

        public async Task<Response<Trainee>> Handle(GetTraineeByIdQuery query, CancellationToken cancellationToken)
        {
           
            var trainee = await _traineeRepository.GetByIdAsync(query.Id);
            if (trainee == null) throw new ApiException($"Room Not Found.");
            return new Response<Trainee>(trainee);
        }
    }

