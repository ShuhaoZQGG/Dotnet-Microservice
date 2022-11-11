using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
  public class PlatformRepo : IPlatformRepo
  {
    private readonly AppDbContext _context;
    public PlatformRepo(AppDbContext context)
    {
      _context = context;
    }
    public async Task<bool> SaveChanges()
    {
      return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<IEnumerable<Platform>> GetAllPlatforms()
    {
      return await _context.Platforms.ToListAsync();
    }

    public async Task<Platform> GetPlatformById(Guid id)
    {
      var platform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);
      if (platform == null)
      {
        throw new KeyNotFoundException("The give id is not found");
      }

      return platform;
    }

    public async Task CreatePlatform(Platform platform)
    {
      if (platform == null)
      {
        throw new ArgumentNullException("Input field cannot be null");
      }

      await _context.Platforms.AddAsync(platform);
    }
  }
}
