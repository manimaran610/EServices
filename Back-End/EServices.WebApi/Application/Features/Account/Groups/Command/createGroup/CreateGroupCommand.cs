using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Account.Groups.Command.createGroup
{
    public class CreateGroupCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
}