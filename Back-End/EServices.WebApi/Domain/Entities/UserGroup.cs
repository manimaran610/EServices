using System;
using Domain.Common;

namespace Domain.Entities
{
    public class UserGroup : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public int GroupId { get; set; }

        public virtual ApplicationUser User { get; set; } = default!;
        public virtual Group Group { get; set; } = default!;


    }
}