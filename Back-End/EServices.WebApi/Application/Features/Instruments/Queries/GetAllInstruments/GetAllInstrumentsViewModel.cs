using System;

namespace Application.Features.Instruments.Queries.GetAllInstruments
{
    public class GetAllInstrumentsViewModel
    {
        public int Id { get; set; }
        public string SerialNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public DateTime CalibratedOn { get; set; }
        public DateTime CalibratedDueOn { get; set; }
        public string CertificateName { get; set; }
    }
}