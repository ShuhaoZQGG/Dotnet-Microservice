using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CommandService.Data
{
  public class CommandRepo : ICommandRepo
  {
	private readonly AppDbContext _context;
	public CommandRepo(AppDbContext context)
	{
	  _context = context;
	}

	public async Task<IEnumerable<Platform>> GetPlatforms()
	{
	  return await _context.Platforms.ToListAsync();
	}

	public async Task CreatePlatform(Platform platform)
	{
	  if (platform == null)
	  {
		throw new ArgumentNullException(nameof(platform));
	  }

	  await _context.Platforms.AddAsync(platform);
	}
	public async Task CreateCommand(Guid platformId, Command command)
	{
	  if (command == null)
	  {
		throw new ArgumentNullException(nameof(command));
	  }

	  if (!await PlatformExists(platformId))
	  {
		throw new ArgumentNullException(platformId.ToString());
	  }

	  command.PlatformId = platformId;
	  await _context.Commands.AddAsync(command);
	}
	public async Task<IEnumerable<Command>> GetCommandsByPlatform(Guid platformId)
	{
      return await _context.Commands
		// this generates a join
		//.Include(c => c.Platform)
		.Where(c => c.PlatformId == platformId)
		//.OrderBy(c => c.Platform.Name)
		.ToListAsync();
    }
	public async Task<Command> GetCommand(Guid platformId, Guid commandId)
	{
	  var command = await _context.Commands
		.Where(c => c.PlatformId == platformId && c.Id == commandId)
		.FirstOrDefaultAsync();

	  if (command == null)
	  {
		throw new ArgumentNullException(nameof(command));
	  }

	  return command;
	}

	public async Task<bool> SaveChanges()
	{
	  return await _context.SaveChangesAsync() > 0;
	}

	public async Task<bool> PlatformExists(Guid platformId)
	{
	  return await _context.Platforms.AnyAsync(p => p.Id == platformId);
	}

	public async Task<bool> ExternalPlatformExists(Guid externalPlatformId)
	{
	  return await _context.Platforms.AnyAsync(p => p.ExternalId == externalPlatformId);
	}
  }
}
