using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Data
{
  public class CommandRepo: ICommandRepo
  {
	private readonly AppDbContext _context;
	public CommandRepo(AppDbContext context)
	{
	  _context = context;
	}

    public IEnumerable<Platform> GetPlatforms()
	{
	  return _context.Platforms.ToList();
	}

	public void CreatePlatform(Platform platform)
	{
	  if (platform == null)
	  {
		throw new ArgumentNullException(nameof(platform));
	  }

	  _context.Platforms.Add(platform);
	}
    public void CreateCommand(Guid platformId,Command command)
	{
	  if (command == null)
	  {
		throw new ArgumentNullException(nameof(command));
	  }

      command.PlatformId = platformId;
	  _context.Commands.Add(command);
    }
    public IEnumerable<Command> GetCommandsByPlatform(Guid platformId)
	{
	  return _context.Commands
		.Where(c => c.PlatformId == platformId)
		.OrderBy(c => c.Platform.Name);
	}
    public Command GetCommand(Guid platformId, Guid commandId)
	{
	  return _context.Commands.Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
	}

    public bool SaveChanges()
	{
	  return _context.SaveChanges() > 0;
	}

	public bool PlatformExists(Guid platformId)
	{
	  return _context.Platforms.Any(p => p.Id == platformId);
	}
  }
}
