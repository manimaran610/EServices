
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
using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Application.Features.Rooms.Commands.CreateRoom
{
    public class GetReportFileByCustDetailIdHandler : IRequestHandler<GetReportFileByCustDetailId, Response<string>>
    {
        private readonly IRoomRepositoryAsync _roomRepository;
        private readonly ICustomerDetailRepositoryAsync _customerDetailRepositoryAsync;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMediator _mediator;
        private List<keyValue> _keyValuePairs = new List<keyValue>();
        public GetReportFileByCustDetailIdHandler
        (
            IRoomRepositoryAsync RoomRepository,
            ICustomerDetailRepositoryAsync customerDetailRepositoryAsync,
            IFileProcessingService fileProcessingService,
            IMediator mediator, IWebHostEnvironment webHostEnvironment
        )
        {
            _roomRepository = RoomRepository;
            _customerDetailRepositoryAsync = customerDetailRepositoryAsync;
            _fileProcessingService = fileProcessingService;
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Response<string>> Handle(GetReportFileByCustDetailId request, CancellationToken cancellationToken)
        {
            this._keyValuePairs = new();
            string processedFile = string.Empty;

            var customerDetail = await _customerDetailRepositoryAsync.GetByIdAsync(request.CustomerDetailId);
            if (customerDetail == null)
                throw new ApiException($"CustomerDetail does not exists with CustomerDetailId -{request.CustomerDetailId}");

            var rooms = await _roomRepository.GetPagedReponseAsync(0, int.MaxValue, $"CustomerDetailId:eq:{request.CustomerDetailId}", "Created:asc", GetRoomSelectExpression());
            if (customerDetail.FormType == FormType.ACPH)
            {
                PopulateACPHKeyValuePairs(customerDetail, rooms);
                await _fileProcessingService.MailMergeWorkDocument(TemplatesBasePath + "ACPH.docx", DestinationBasePath + "ACPH-out.docx", _keyValuePairs);
                processedFile = ConvertFileToBase64(DestinationBasePath + "ACPH-out.docx");
            }


            return new Response<string>(processedFile, "File Processed successfully");
        }

        private void PopulateACPHKeyValuePairs(CustomerDetail customerDetail, IReadOnlyList<Room> rooms)
        {
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
                        this._keyValuePairs.Add(new(keyPrefix + (i + 1), airvelocityRdng[i]));
                    }
                    MapPropertiesToKeyValuePair(grill, keyPrefix);
                });
            }
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

        private string ConvertFileToBase64(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }
        private string TemplatesBasePath => _webHostEnvironment.ContentRootPath.Split("\\WebApi")[0] + "\\Application\\Features\\ReportFiles\\WordTemplates\\";
        private string DestinationBasePath => _webHostEnvironment.ContentRootPath.Split("\\WebApi")[0] + "\\Application\\Features\\ReportFiles\\ProcessedFiles\\";


    }
}