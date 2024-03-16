using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public class CreateGroupRequest
    {
        public string Name { get; set; }
        public string Location { get; set; }  
    }
}