using CommandService.Models;

namespace CommandService.Data
{
  public interface ICommandRepo
  {
    bool SaveChanges();
    IEnumerable<Platform> GetPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatformExists(Guid platformId);
    IEnumerable<Command> GetCommandsByPlatform(Guid platformId);
    Command GetCommand(Guid platformId, Guid commandId);
    void CreateCommand(Guid platformId, Command command);
  }
}
