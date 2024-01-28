

using System;
using Application.Wrappers;
using Domain.Enums;
using MediatR;

namespace Application.Features.CustomerDetails.Commands.UpdateCustomerDetail
{
    public class UpdateCustomerDetailCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int TraineeId { get; set; }
        public string Client { get; set; }
        public string plant { get; set; }
        public string TestCondition { get; set; }
        public string ClassType { get; set; }

        public string EquipmentId { get; set; }
        public string AreaOfTest { get; set; }
        public FormType FormType { get; set; }
        public DateTime DateOfTest { get; set; }
        public DateTime DateOfTestDue { get; set; }

        public int InstrumentId { get; set; }
    }
}