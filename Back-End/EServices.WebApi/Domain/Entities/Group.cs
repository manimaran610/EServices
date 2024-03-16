using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Group : AuditableBaseEntity
    {
        public Guid UniqueId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Location { get; set; }
       public virtual List<UserGroup> GroupUsers { get; set; } = default!;


    }
}