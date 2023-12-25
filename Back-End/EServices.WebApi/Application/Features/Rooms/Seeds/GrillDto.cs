
namespace Application.Features.Rooms.Seeds
{
    public class GrillDTO
    {
        public string ReferenceNumber { get; set; }
        public float FilterAreaSqft { get; set; }
        public string AirVelocityReadingInFPMO { get; set; }
        public int AvgVelocityFPM { get; set; }
        public int AirFlowCFM { get; set; }
        public int Penetration { get; set; }
        public int Effective { get; set; }
        public string UpStreamConcat { get; set; }
        public string Result { get; set; }
    }
}