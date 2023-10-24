

using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Instrument :AuditableBaseEntity
    {
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime CalibratedOn { get; set; }
        public DateTime CalibratedDueOn { get; set; }
        public string CertificateName {get;set;}
        public byte[]  CertificateFile {get;set;}

    }
}