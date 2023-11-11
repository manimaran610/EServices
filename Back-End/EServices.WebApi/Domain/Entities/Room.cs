

using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Room : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string DesignACPH { get; set; }
        public int NoOfGrills { get; set; }
        public int RoomVolume { get; set; }
        public int TotalAirFlowCFM { get; set; }
        public int AirChangesPerHour { get; set; }
        public int CustomerDetailId { get; set; }
        
        public virtual CustomerDetail CustomerDetail { get; set; } = default!;
        public virtual List<RoomGrill> RoomGrills { get; set; } = default;



    }
}