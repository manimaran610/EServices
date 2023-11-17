
using System.Threading;
using System.Threading.Tasks;
using Application.Features.DomainEvents.RoomGrillsAddRangeEvent;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Features.ReportFiles.GetReportFileByCustDetailId;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Common;

namespace Application.Features.Rooms.Commands.CreateRoom
{
    public class GetReportFileByCustDetailIdHandler : IRequestHandler<GetReportFileByCustDetailId, Response<int>>
    {
        private readonly IRoomRepositoryAsync _roomRepository;
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepositoryAsync;
        private readonly IRoomGrillRepositoryAsync _roomGrillRepositoryAsync;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private List<keyValue> _keyValuePairs = new List<keyValue>();
        public GetReportFileByCustDetailIdHandler
        (
            IRoomRepositoryAsync RoomRepository,
            ICustomerDetailRepositoryAsync customerDetailRepositoryAsync,
            IRoomGrillRepositoryAsync roomGrillRepositoryAsync,
            IMapper mapper,
            IMediator mediator
        )
        {
            _roomRepository = RoomRepository;
            _customerDetailRepositoryAsync = customerDetailRepositoryAsync;
            _roomGrillRepositoryAsync = roomGrillRepositoryAsync;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response<int>> Handle(GetReportFileByCustDetailId request, CancellationToken cancellationToken)
        {
            this._keyValuePairs = new();

            var customerDetail = await _customerDetailRepositoryAsync.GetByIdAsync(request.CustomerDetailId);
            if (customerDetail == null)
                throw new ApiException($"CustomerDetail does not exists with CustomerDetailId -{request.CustomerDetailId}");

            var rooms = await _roomRepository.GetPagedReponseAsync(0, int.MaxValue, $"CustomerDetailId:eq:{request.CustomerDetailId}", "Created:asc", GetRoomSelectExpression());

            // if (customerDetail.FormType == FormType.ACPH)
            // {
            MapPropertiesToKeyValuePair(customerDetail);
            for (int i = 0; i < rooms.Count; i++)
            {
                string keyPrefix = $"R[{i}]-";
                MapPropertiesToKeyValuePair(rooms[i], keyPrefix);
                rooms[i].RoomGrills.ForEach(grill =>
                {
                    var airvelocityRdng = grill.AirVelocityReadingInFPMO.Split(',');
                    for (int i = 0; i < airvelocityRdng.Length; i++)
                    {
                        this._keyValuePairs.Add(new(keyPrefix + (i+1), airvelocityRdng[i]));
                    }
                    MapPropertiesToKeyValuePair(grill, keyPrefix);
                });
            }
            // }
            return new Response<int>(1);
        }


        private Expression<Func<Room, Room>> GetRoomSelectExpression()
        {
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
                RoomGrills = e.RoomGrills.Where(e => !e.IsDeleted).ToList()

            };

            return selectExpression;
        }

        private void MapPropertiesToKeyValuePair<T>(T obj, string keyPrefix = "")
        {
            if (obj != null)
            {
                obj.GetType().GetProperties().ToList().ForEach(prop =>
                {
                    this._keyValuePairs.Add(new(keyPrefix + prop.Name, Convert.ToString(prop.GetValue(obj, default))));
                });
            }
        }
    }
}