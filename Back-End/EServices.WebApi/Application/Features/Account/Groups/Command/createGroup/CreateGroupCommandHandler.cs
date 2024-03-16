using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Account.Groups.Command.createGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Response<int>>
    {
        private readonly IGroupRepositoryAsync _groupRepository;
        private readonly IMapper _mapper;
        public CreateGroupCommandHandler(IGroupRepositoryAsync InstrumentRepository, IMapper mapper)
        {
            _groupRepository = InstrumentRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = _mapper.Map<Domain.Entities.Group>(request);
            var existingGroups = await _groupRepository.GetPagedReponseAsync(0, 1, $"Name:eq:{request.Name}");
           
            if (existingGroups.Any())
                throw new ApiException($"Group already exists with name - {request.Name}");
                
            await _groupRepository.AddAsync(group);
            return new Response<int>(group.Id);
        }

    }
}