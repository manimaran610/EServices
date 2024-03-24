
using System.Threading.Tasks;
using Application.DTOs.Account;
using Application.Features.Account.Groups.Command.createGroup;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class GroupController : BaseApiController
    {       
         private readonly IAccountService _accountService;

        private readonly ILogger<GroupController> _logger;

        public GroupController(ILogger<GroupController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateGroupCommand command)
        {
            return Created("Created", await Mediator.Send(command));
        }

         [HttpPost("create-user")]
         // [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> CreateNewUser(CreateManagementUserRequest model)
        {
           var origin = Request.Headers["origin"];
            return Ok(await _accountService.CreateManagementUserAsync(model,origin));
        }

    }
}