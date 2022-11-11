using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PlatformService.Models;

namespace PlatformService.Data
{
  public static class PrepDb
  {
    public static void PrepPopulation(WebApplication app, bool isProd)
    {
      using (var serviceScope = app.Services.CreateScope())
      {
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
      }
    }

    public static void SeedData(AppDbContext context, bool isProd)
    {
      try
      {
        if (isProd)
        {
          Console.WriteLine("--> Attempting to apply migrations...");
          try
          {
            //context.Database.Migrate();
            context.Database.EnsureCreated();
          }
          catch (Exception ex)
          {
            Console.WriteLine($"--> Could not run migrations: {ex.Message}");
          }
        }

        if (!context.Platforms.Any())
        {
          Console.WriteLine("--> Seeding Data...");

          context.Platforms.AddRange(
              new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
              new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
              new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
          );

          context.SaveChanges();
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
