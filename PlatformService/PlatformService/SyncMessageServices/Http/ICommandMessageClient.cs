using PlatformService.Dtos;

namespace PlatformService.SyncMessageServices.Http
{
  public interface ICommandMessageClient
  {
    Task SendPlatformToCommand(PlatformReadDto platform);
  }
}
