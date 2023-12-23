using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Trainees.Queries.GetAllTrainees
{
    public class GetAllTraineesQueryHandler : IRequestHandler<GetAllTraineesQuery, PagedResponse<IEnumerable<GetAllTraineesViewModel>>>
    {
        private readonly ITraineeRepositoryAsync _traineeRepository;
        private readonly IMapper _mapper;
        public GetAllTraineesQueryHandler(ITraineeRepositoryAsync TraineeRepository, IMapper mapper)
        {
            _traineeRepository = TraineeRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllTraineesViewModel>>> Handle(GetAllTraineesQuery request, CancellationToken cancellationToken)
        {

            var reqParam = _mapper.Map<RequestParameter>(request);
            Expression<System.Func<Trainee, Trainee>> selectExpression = e => new()
            {
                Id = e.Id,
                Name = e.Name,
                EmployeeId=e.EmployeeId,
                CertificateName=e.CertificateName
            };

            var TraineePagedResponse = await _traineeRepository.GetPagedReponseAsync(reqParam.Offset, reqParam.Count, reqParam.Filter, reqParam.Sort, selectExpression);
            var totalCount = await _traineeRepository.TotalCountAsync();

            var TraineeViewModel = _mapper.Map<IEnumerable<GetAllTraineesViewModel>>(TraineePagedResponse);
            return new PagedResponse<IEnumerable<GetAllTraineesViewModel>>(TraineeViewModel, request.Offset, request.Count, TraineeViewModel.Count(), totalCount);
        }
    }
}