using Microsoft.Extensions.Options;
using PlatformService.Configuration;
using PlatformService.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncMessageServices.Http
{
  public class CommandMessageClient : ICommandMessageClient
  {
    private readonly HttpClient _httpClient;
    private readonly string _platformsUrl;
    private readonly ILogger<CommandMessageClient> _logger;
    public CommandMessageClient(HttpClient httpClient, IOptions<CommandService> CommandServiceConfig, ILogger<CommandMessageClient> logger)
    {
      _httpClient = httpClient;
      _platformsUrl = CommandServiceConfig.Value.PlatformsUrl;
      _logger = logger;
      _logger.LogInformation("platformUrl: " + _platformsUrl);
    }
    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
      var httpContent = new StringContent(
          JsonSerializer.Serialize(platform),
          Encoding.UTF8,
          "application/json"
         );
      var response = await _httpClient.PostAsync(_platformsUrl, httpContent);
      if (response.IsSuccessStatusCode)
      {
        Console.WriteLine("--> Sync Post to Command Service is OK");
      }
      else
      {
        Console.WriteLine("--> Sync Post to Command Service is Failed");
      }
    }
  }
}
