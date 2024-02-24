using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Trainees.Queries.GetTraineesById;

public class GetTraineeByIdQuery: IRequest<Response<Trainee>>
    {
        public int Id { get; set; }
    }

