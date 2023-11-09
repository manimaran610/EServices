

using System;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class CustomerDetail : AuditableBaseEntity
    {
        public string Client { get; set; }
        public string plant { get; set; }
        public string EquipmentId { get; set; }
        public string AreaOfTest { get; set; }
        public FormType FormType {get;set;}
        public DateTime DateOfTest { get; set; }
        public int InstrumentId { get; set; }
        public virtual Instrument Instrument { get; set; } =default!;


}
}