using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.MessageBroker;
using PlatformService.Models;
using PlatformService.SyncMessageServices.Http;

namespace PlatformService.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  // [controller] will get the portion of the class name except for Controller, so here it will get name Platforms
  public class PlatformsController : ControllerBase
  {
    private readonly IPlatformRepo _repo;
    private readonly IMapper _mapper;
    private readonly IMessageHttpClient _messageHttpClient;
    private readonly IMessageBusClient _messageBusClient;
    private readonly ILogger<PlatformsController> _logger;
    public PlatformsController(IPlatformRepo repository, IMapper mapper, IMessageHttpClient commandMessageClient, IMessageBusClient messageBusClient, ILogger<PlatformsController> logger)
    {
      _repo = repository;
      _mapper = mapper;
      _messageHttpClient = commandMessageClient;
      _messageBusClient = messageBusClient;
      _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatforms()
    {
      var platforms = await _repo.GetAllPlatforms();
      return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public async Task<ActionResult<PlatformReadDto>> GetPlatformById(Guid id)
    {
      var platform = await _repo.GetPlatformById(id);
      if (platform == null)
      {
        return NotFound();
      }
      return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
      Platform platform = _mapper.Map<Platform>(platformCreateDto);
      await _repo.CreatePlatform(platform);
      await _repo.SaveChanges();
      PlatformReadDto platformReadDto = _mapper.Map<PlatformReadDto>(platform);
      // nameof(GetPlatformById) will return what we defined at line 29 Name = "GetPlatformById"
      
      // send message through http client
      try
      {
        await _messageHttpClient.SendPlatformToCommand(platformReadDto);
      }
      catch (Exception e)
      {
        _logger.LogError("---> Could not send the message syncronously: " + e.Message);
        return StatusCode(500, e);
      }

      // send message through rabbitmq client
      try
      {
        var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
        _logger.LogInformation($"platformPublishedDto, {platformPublishedDto.Id}, {platformPublishedDto.Name}");
        platformPublishedDto.Event = "Platform Published";
        _logger.LogInformation($"platformPublishedDto event, {platformPublishedDto.Event}");
        // To-do: specify exchange and routing key
        _messageBusClient.PublishNewPlatform(platformPublishedDto, "trigger", "");
      }
      catch (Exception ex)
      {
        _logger.LogError($"---> Could not send the message asyncronously: {ex.Message}");
        return StatusCode(500, ex.Message);
      }

      return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
    }
  }
}
