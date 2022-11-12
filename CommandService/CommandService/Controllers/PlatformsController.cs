using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
  // need to change this later, as this will have conflict with the route in platformservice when we use api gateway
  [Route("api/v1/c/[controller]")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    private readonly ILogger<PlatformsController> _logger;
    public PlatformsController(ILogger<PlatformsController> logger)
    {
      _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> TestInboundConnection()
    {
      _logger.LogInformation("--> Received Message");
      return Ok();
    }
  }
}
