using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static async Task PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                await SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static async Task SeedData(AppDbContext context)
        {
            if (!await context.Platforms.AnyAsync())
            {
                Console.WriteLine("--> Seeding Data");
                await context.Platforms.AddRangeAsync(
                new Platform()
                {
                    Name = "Dotnet",
                    Publisher = "Microsoft",
                    Cost = "Free",
                },
                new Platform()
                {
                    Name = "SQL Server",
                    Publisher = "Microsoft",
                    Cost = "Free"
                },
                new Platform()
                {
                    Name = "Kubernetes",
                    Publisher = "Cloud Native Computing Fondation",
                    Cost = "Free"
                }
                );
                await context.SaveChangesAsync();
            } 
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
