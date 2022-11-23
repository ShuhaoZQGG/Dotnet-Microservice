using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
  [Route("api/v1/c/platforms/{platformId}/[controller]")]
  [ApiController]
  public class CommandsController : ControllerBase
  {
    private readonly ILogger<CommandsController> _logger;
    private readonly IMapper _mapper;
    private readonly ICommandRepo _repo;

    public CommandsController(ILogger<CommandsController> logger, IMapper mapper, ICommandRepo repo)
    {
      _logger = logger;
      _mapper = mapper;
      _repo = repo;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetAllCommandsByPlatformId(Guid platformId)
    {
      _logger.LogInformation($"---> Get all commands for platform id {platformId}");
      try
      {
        var commands = await _repo.GetCommandsByPlatform(platformId);
        if (commands == null)
        {
          return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error, {ex.Message}");
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public async Task<ActionResult<CommandReadDto>> GetCommandForPlatform(Guid platformId, Guid commandId)
    {
      _logger.LogInformation($"---> Get Command Id {commandId} for Platform with id {platformId}");
      try
      {
        var command = await _repo.GetCommand(platformId, commandId);
        if (command == null)
        {
          return NotFound();
        } 
        return Ok(_mapper.Map<CommandReadDto>(command));
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error: {ex.Message}");
        return BadRequest(ex.Message);
      }
    }

    [HttpPost]
    public async Task<ActionResult<CommandReadDto>> CreateCommandForPlatform(Guid platformId, [FromForm] CommandCreateDto commandCreateDto)
    {
      _logger.LogInformation($"---> Create a Command {commandCreateDto.CommandLine} for platform id {platformId}");
      try
      {
        var command = _mapper.Map<Command>(commandCreateDto);
        await _repo.CreateCommand(platformId, command);
        await _repo.SaveChanges();
        var commandReadDto = _mapper.Map<CommandReadDto>(command);
        return 
          CreatedAtRoute(nameof(GetCommandForPlatform),
          new { platformId = platformId, commandId = commandReadDto.Id },
          commandReadDto);
      } 
      catch (Exception ex)
      {
        _logger.LogError($"Error: {ex.Message}");
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("test")]
    public async Task<ActionResult> CreatePlatformId(Guid platformId)
    {
      try
      {
        _logger.LogInformation(platformId.ToString());
        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error: {ex.Message}");
        return BadRequest(ex.Message);
      }
    }
  }
}
