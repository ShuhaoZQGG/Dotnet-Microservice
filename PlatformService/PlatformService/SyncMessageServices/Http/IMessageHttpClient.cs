using PlatformService.Dtos;

namespace PlatformService.SyncMessageServices.Http
{
  public interface IMessageHttpClient
  {
    Task SendPlatformToCommand(PlatformReadDto platform);
  }
}
