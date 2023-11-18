
using System.Threading.Tasks;
using Application.Features.CustomerDetails.Commands.CreateCustomerDetail;
using Application.Features.CustomerDetails.Commands.DeleteCustomerDetail;
using Application.Features.CustomerDetails.Commands.UpdateCustomerDetail;
using Application.Features.CustomerDetails.Queries.GetAllCustomerDetails;
using Application.Features.CustomerDetails.Queries.GetCustomerDetailById;
using Application.Features.ReportFiles.GetReportFileByCustDetailId;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CustomerDetailController : BaseApiController
    {
        //GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllCustomerDetailsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetCustomerDetailByIdQuery { Id = id }));
        }

         // GET Report api/<controller>/5
        [HttpGet("{id}/Report")]
        public async Task<IActionResult> GenerateReport(int id)
        {
            return Ok(await Mediator.Send(new GetReportFileByCustDetailId { CustomerDetailId = id }));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateCustomerDetailCommand command)
        {
            return Created("Created",await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,UpdateCustomerDetailCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Invalid Id");
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCustomerDetailCommand { Id = id }));
        }
    }
}
