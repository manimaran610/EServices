using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Trainee: AuditableBaseEntity
    {
        public string Name { get; set; }
        public string CertificateName { get; set; }
        public string CertificateFile { get; set; }

        public virtual List<CustomerDetail> CustomerDetails { get; set; } = default;

    }

}