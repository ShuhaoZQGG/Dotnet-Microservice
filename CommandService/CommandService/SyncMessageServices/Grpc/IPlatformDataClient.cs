using CommandService.Models;

namespace CommandService.SyncMessageServices.Grpc
{
  public interface IPlatformDataClient
  {
    Task<IEnumerable<Platform>> ReturnAllPlatforms();
  }
}
