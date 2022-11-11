using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PlatformService.Models;

namespace PlatformService.Data
{
  public static class PrepDb
  {
    public static async Task PrepPopulation(WebApplication app, bool isProd)
    {
      using (var serviceScope = app.Services.CreateAsyncScope())
      {
        await SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
      }
    }

    public static async Task SeedData(AppDbContext context, bool isProd)
    {
      try
      {
        if (isProd)
        {
          Console.WriteLine("--> Attempting to apply migrations...");
          try
          {
            //context.Database.Migrate();
            await context.Database.EnsureCreatedAsync();
          }
          catch (Exception ex)
          {
            Console.WriteLine($"--> Could not run migrations: {ex.Message}");
          }
        }

        if (!await context.Platforms.AnyAsync())
        {
          Console.WriteLine("--> Seeding Data...");

          await context.Platforms.AddRangeAsync(
              new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
              new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
              new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
          );

          await context.SaveChangesAsync();
        }
        else
        {
          Console.WriteLine("--> We already have data");
        }
      } catch (Exception ex)
      {
        Console.Error.WriteLine($"Error: {ex.Message}");
      }
    }
  }
}
