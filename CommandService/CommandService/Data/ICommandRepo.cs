using CommandService.Models;

namespace CommandService.Data
{
  public interface ICommandRepo
  {
    Task<bool> SaveChanges();
    Task<IEnumerable<Platform>> GetPlatforms();
    Task CreatePlatform(Platform platform);
    Task<bool> PlatformExists(Guid platformId);
    Task<bool> ExternalPlatformExists(Guid ExternalPlatformId);
    Task<IEnumerable<Command>> GetCommandsByPlatform(Guid platformId);
    Task<Command> GetCommand(Guid platformId, Guid commandId);
    Task CreateCommand(Guid platformId, Command command);
  }
}
