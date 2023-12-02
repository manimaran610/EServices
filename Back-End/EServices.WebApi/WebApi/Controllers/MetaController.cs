using Application.Features.Instruments.Queries.GetAllInstruments;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class MetaController : BaseApiController
    {
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }

          //GET: api/<controller>
        [HttpGet("/Logs")]
        public async Task<IActionResult> Get([FromQuery] GetAllLogsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

    }
}
