
using Application.Wrappers;
using MediatR;

namespace Application.Features.ReportFiles.GetReportFileByCustDetailId
{
    public class GetReportFileByCustDetailId : IRequest<Response<object>>
    {
        public int CustomerDetailId { get; set; }
    }
}