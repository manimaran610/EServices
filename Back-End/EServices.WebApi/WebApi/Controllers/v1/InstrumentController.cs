
using System.Threading.Tasks;
using Application.Features.Instruments.Commands.CreateInstrument;
using Application.Features.Instruments.Commands.DeleteInstrument;
using Application.Features.Instruments.Commands.UpdateInstrument;
using Application.Features.Instruments.Queries.GetAllInstruments;
using Application.Features.Instruments.Queries.GetInstrumentById;
using Application.Features.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class InstrumentController : BaseApiController
    {
        //GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllInstrumentsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetInstrumentByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] CreateInstrumentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromQuery] UpdateInstrumentCommand command)
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
            return Ok(await Mediator.Send(new DeleteInstrumentCommand { Id = id }));
        }
    }
}
