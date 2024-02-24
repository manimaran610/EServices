
using System.Threading.Tasks;
using Application.Features.Trainees.Commands.CreateTrainee;
using Application.Features.Trainees.Queries.GetAllTrainees;
using Application.Features.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Trainees.Queries.GetTraineesById;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class TraineeController : BaseApiController
    {
        //GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllTraineesQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetTraineeByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateTraineeCommand command)
        {
            return Created("Created",await Mediator.Send(command));
        }


      
    }
}
