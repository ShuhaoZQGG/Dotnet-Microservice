using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
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
    private readonly ICommandRepo _repo;
    private readonly IMapper _mapper;
    public PlatformsController(ILogger<PlatformsController> logger, ICommandRepo repo, IMapper mapper)
    {
      _logger = logger;
      _repo = repo;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatforms()
    {
      try
      {
        _logger.LogInformation("==> Getting Platforms from Commands Service");
        var platformItems = await _repo.GetPlatforms();
        if (platformItems == null)
        {
          return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
      } catch (Exception ex)
      {
        _logger.LogError($"Error, {ex.Message}");
        return BadRequest();
      }
    } 

    [HttpPost]
    public async Task<ActionResult> TestInboundConnection()
    {
      _logger.LogInformation("--> Received Message");
      return Ok();
    }
  }
}
