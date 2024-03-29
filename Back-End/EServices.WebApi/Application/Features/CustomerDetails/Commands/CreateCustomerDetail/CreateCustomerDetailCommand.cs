using System;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.CustomerDetails.Commands.CreateCustomerDetail
{
    public class CreateCustomerDetailCommand : IRequest<Response<int>>
    {
        public string Client { get; set; }
        public string Limit { get; set; }
        public string plant { get; set; }
        public string EquipmentId { get; set; }
        public string TestCondition { get; set; }
        public string ClassType { get; set; }
        public string AreaOfTest { get; set; }
        public FormType FormType { get; set; }
        public DateTime DateOfTest { get; set; }
        public DateTime DateOfTestDue { get; set; }

        public int TraineeId { get; set; }
        public int InstrumentId { get; set; }
    }
}
