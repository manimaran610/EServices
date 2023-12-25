

using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class RoomGrill : AuditableBaseEntity
    {
        public string ReferenceNumber { get; set; }
        public float FilterAreaSqft { get; set; }
        public string AirVelocityReadingInFPMO { get; set; }
        public int AvgVelocityFPM { get; set; }
        public int AirFlowCFM { get; set; }
        public int RoomId { get; set; }
        public int Size { get; set; }
        public int Penetration { get; set; }
        public int Effective { get; set; }
        public string UpStreamConcat { get; set; }
        public string Result { get; set; }


        public virtual Room Room { get; set; } = default!;

    }
}