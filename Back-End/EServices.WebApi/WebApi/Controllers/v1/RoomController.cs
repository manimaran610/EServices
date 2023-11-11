
using System.Threading.Tasks;
using Application.Features.Rooms.Commands.CreateRoom;
using Application.Features.Rooms.Commands.DeleteRoom;
using Application.Features.Rooms.Commands.UpdateRoom;
using Application.Features.Rooms.Queries.GetAllRooms;
using Application.Features.Rooms.Queries.GetRoomById;
using Application.Features.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class RoomController : BaseApiController
    {
        //GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllRoomsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetRoomByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateRoomCommand command)
        {
            return Created("Created",await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,UpdateRoomCommand command)
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
            return Ok(await Mediator.Send(new DeleteRoomCommand { Id = id }));
        }
    }
}
