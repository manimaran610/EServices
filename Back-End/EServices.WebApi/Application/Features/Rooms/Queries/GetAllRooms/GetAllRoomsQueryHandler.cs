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

namespace Application.Features.Rooms.Queries.GetAllRooms
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, PagedResponse<IEnumerable<GetAllRoomsViewModel>>>
    {
        private readonly IRoomRepositoryAsync _roomRepository;
        private readonly IMapper _mapper;
        public GetAllRoomsQueryHandler(IRoomRepositoryAsync RoomRepository, IMapper mapper)
        {
            _roomRepository = RoomRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllRoomsViewModel>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {

            var reqParam = _mapper.Map<RequestParameter>(request);
            Expression<Func<Room, Room>> selectExpression = e => new()
            {
                Id = e.Id,
                DesignACPH = e.DesignACPH,
                AirChangesPerHour = e.AirChangesPerHour,
                NoOfGrills = e.NoOfGrills,
                RoomVolume = e.RoomVolume,
                CustomerDetailId = e.CustomerDetailId,
                Name = e.Name,
                TotalAirFlowCFM = e.TotalAirFlowCFM,
                RoomGrills = e.RoomGrills

            };

            var RoomPagedResponse = await _roomRepository.GetPagedReponseAsync(reqParam.Offset, reqParam.Count, reqParam.Filter, reqParam.Sort, selectExpression);
            var totalCount = await _roomRepository.TotalCountAsync();

            var RoomViewModel = _mapper.Map<IEnumerable<GetAllRoomsViewModel>>(RoomPagedResponse);
            return new PagedResponse<IEnumerable<GetAllRoomsViewModel>>(RoomViewModel, request.Offset, request.Count, RoomViewModel.Count(), totalCount);
        }
    }
}