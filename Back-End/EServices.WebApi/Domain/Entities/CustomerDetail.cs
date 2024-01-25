using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class CustomerDetail : AuditableBaseEntity
    {
        public string CustomerNo { get; set; }
        public string Limit { get; set; }
        public string Client { get; set; }
        public string TestReference { get; set; }
        public string ClassType { get; set; }
        public string Plant { get; set; }
        public string EquipmentId { get; set; }
        public string AreaOfTest { get; set; }
        public FormType FormType { get; set; }
        public DateTime DateOfTest { get; set; }
        public DateTime DateOfTestDue { get; set; }
        public int InstrumentId { get; set; }
        public int? TraineeId { get; set; }
        public virtual Instrument Instrument { get; set; } = default!;
        public virtual Trainee Trainee { get; set; } = default!;
        public virtual List<Room> Rooms { get; set; } = default;
    }
}
