using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Instruments.Commands.DeleteInstrument
{
    public class DeleteInstrumentCommand: IRequest<Response<int>>
    {
        public int Id { get; set; }
        
    }
}