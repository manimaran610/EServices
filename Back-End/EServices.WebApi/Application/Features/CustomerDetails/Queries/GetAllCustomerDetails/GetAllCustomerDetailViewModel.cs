using System;
using Domain.Enums;

namespace Application.Features.CustomerDetails.Queries.GetAllCustomerDetails
{
    public class GetAllCustomerDetailsViewModel
    {
        public int Id { get; set; }
        public int TraineeId { get; set; }
        public string Client { get; set; }
        public string CustomerNo { get; set; }
        public string TestReference { get; set; }
        public string ClassTypes { get; set; }
        public string plant { get; set; }
        public string EquipmentId { get; set; }
        public string AreaOfTest { get; set; }
        public FormType FormType { get; set; }
        public DateTime DateOfTest { get; set; }
        public DateTime DateOfTestDue { get; set; }
        public int InstrumentId { get; set; }
    }
}