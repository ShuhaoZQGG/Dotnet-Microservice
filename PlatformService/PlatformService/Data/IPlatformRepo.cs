using PlatformService.Models;

namespace PlatformService.Data
{
  public interface IPlatformRepo
  {
    Task<bool> SaveChanges();
    Task<IEnumerable<Platform>> GetAllPlatforms();
    Task<Platform> GetPlatformById(Guid id);
    Task CreatePlatform(Platform platform);
  }
}
