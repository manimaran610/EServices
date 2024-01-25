using Domain.Common;

namespace Domain.Entities
{
    public class RoomLocation : AuditableBaseEntity
    {
        public string ReferenceNumber { get; set; }
        public string Condition { get; set; }
        public string Time { get; set; }
        public string PointFiveMicronCycles { get; set; }
        public string OneMicronCycles { get; set; }
        public string FiveMicronCycles { get; set; }

        public int AveragePointFiveMicron { get; set; }
        public int AverageOneMicron { get; set; }
        public int AverageFiveMicron { get; set; }

        public string Result { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; } = default!;
    }
}
