

using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Instrument : AuditableBaseEntity
    {
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public DateTime CalibratedOn { get; set; }
        public DateTime CalibratedDueOn { get; set; }
        public string CertificateName { get; set; }
        public string CertificateFile { get; set; }

        public virtual List<CustomerDetail> CustomerDetails { get; set; } = default;

    }
}