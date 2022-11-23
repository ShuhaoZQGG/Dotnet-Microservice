using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.Event
{
  public class Event : IEvent
  {
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<Event> _logger;

    public Event(IServiceScopeFactory serviceScopeFactory, IMapper mapper, ILogger<Event> logger)
    {
      _serviceScopeFactory = serviceScopeFactory;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task ProcessEvent(string message)
    {
      var eventType = DeterminEventType(message);

      switch (eventType)
      {
        case EventType.PlatformPublished:
          await addPlatform(message);
          break;
        default:
          break;
      }
    }

    private EventType DeterminEventType(string notificationMessage)
    {
      _logger.LogInformation("--> Determine Event");
      var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
      switch (eventType.Event)
      {
        case "Platform_Published":
          _logger.LogInformation("---> Platform Published Event detected");
          return EventType.PlatformPublished;
        default:
          _logger.LogInformation("---> Could not determine the event type");
          return EventType.Undetermined;
      }
    }

    private async Task addPlatform(string platformPublishedMessage)
    {
      using (var scope = _serviceScopeFactory.CreateScope())
      {
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
        try
        {
          var plat = _mapper.Map<Platform>(platformPublishedDto);
          if (!await repo.ExternalPlatformExists(plat.ExternalId))
          {
            await repo.CreatePlatform(plat);
            await repo.SaveChanges();
            _logger.LogInformation($"Platform added {plat.Name}");
          } 
          else
          {
            _logger.LogInformation("Platform already exists");
          }
        }
        catch (Exception ex)
        {
          _logger.LogError($"Error Could not add Platform to DB: {ex.Message}");
        }
      }
    }
    enum EventType
    {
      PlatformPublished,
      Undetermined
    }
  }
}
