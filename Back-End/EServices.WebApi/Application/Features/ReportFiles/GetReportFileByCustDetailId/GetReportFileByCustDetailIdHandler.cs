
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

            var customerDetail = await _customerDetailRepositoryAsync.GetByIdAsync(request.CustomerDetailId, GetCustomerDetailSelectExpression());
            if (customerDetail == null)
                throw new ApiException($"CustomerDetail does not exists with CustomerDetailId -{request.CustomerDetailId}");

            var rooms = await _roomRepository.GetPagedReponseAsync(0, int.MaxValue, $"CustomerDetailId:eq:{request.CustomerDetailId}", "Name:asc", GetRoomSelectExpression());
            if (customerDetail.FormType == FormType.ACPH)
            {
                var templateRows = PopulateACPHTemplateRowConfigs(rooms);
                PopulateACPHKeyValuePairs(customerDetail, rooms);
                await _fileProcessingService.MailMergeWorkDocument(GetFullPath("ACPH.docx"), GetFullPath("ACPH-out.docx"), _keyValuePairs, templateRows);
                processedFile = ConvertFileToBase64(GetFullPath("ACPH-out.docx"));
            }


            return new Response<string>(processedFile, "File Processed successfully");
        }

        private void PopulateACPHKeyValuePairs(CustomerDetail customerDetail, IReadOnlyList<Room> rooms)
        {
            MapPropertiesToKeyValuePair(customerDetail);
            MapPropertiesToKeyValuePair(customerDetail.Instrument);
             _keyValuePairs.Add(new keyValue("c-due", customerDetail.DateOfTestDue.ToString()));
            if (customerDetail.Trainee != null) _keyValuePairs.Add(new keyValue("TestedBy", customerDetail.Trainee.Name));

            for (int i = 1; i <= rooms.Count; i++)
            {
                string keyPrefix = $"R-";
                MapPropertiesToKeyValuePair(rooms[i - 1], keyPrefix);
                rooms[i - 1].RoomGrills.ForEach(grill =>
                {
                    var airvelocityRdng = grill.AirVelocityReadingInFPMO.Split(',');
                    for (int j = 0; j < airvelocityRdng.Length; j++)
                    {
                        this._keyValuePairs.Add(new(keyPrefix + (j + 1), airvelocityRdng[j]));
                    }
                    MapPropertiesToKeyValuePair(grill, keyPrefix);
                });
            }
        }


        private List<TemplateRowConfig> PopulateACPHTemplateRowConfigs(IReadOnlyList<Room> rooms)
        {
            List<TemplateRowConfig> result = new();
            int orderNo = 1;
            foreach (var room in rooms)
            {
                result.Add(new(orderNo, 3, 4, room.RoomGrills.Count()));
                orderNo++;
            }

            return result.OrderByDescending(e => e.OrderNo).ToList();


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
                RoomGrills = e.RoomGrills.Where(e => !e.IsDeleted).OrderBy(e => e.ReferenceNumber).ToList()

            };

            return selectExpression;
        }

        private Expression<Func<CustomerDetail, CustomerDetail>> GetCustomerDetailSelectExpression()
        {
            Expression<Func<CustomerDetail, CustomerDetail>> expression = e => new()
            {
                Id = e.Id,
                Client = e.Client,
                CustomerNo=e.CustomerNo,
                AreaOfTest = e.AreaOfTest,
                DateOfTest = e.DateOfTest,
                EquipmentId = e.EquipmentId,
                FormType = e.FormType,
                InstrumentId = e.InstrumentId,
                Plant = e.Plant,
                ClassType = e.ClassType,
                TestReference = e.TestReference,
                DateOfTestDue = e.DateOfTestDue,
                Instrument = e.Instrument,
                Trainee = e.Trainee
            };
            return expression;
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

        private string GetFullPath(string filename) => Path.Combine("WordTemplates", filename);


    }
}