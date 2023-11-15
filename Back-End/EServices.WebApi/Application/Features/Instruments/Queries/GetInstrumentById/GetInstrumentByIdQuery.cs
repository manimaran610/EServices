
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Instruments.Queries.GetInstrumentById
{
    public class GetInstrumentByIdQuery : IRequest<Response<Instrument>>
    {
        public int Id { get; set; }
    }
}