using CommandService.Models;
using CommandService.SyncMessageServices.Grpc;

namespace CommandService.Data
{
  public class PrepDb
  {

    public static async Task PrepPopulation(WebApplication app)
    {
      using (var serviceScope = app.Services.CreateAsyncScope())
      {
        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

        var platforms = await grpcClient.ReturnAllPlatforms();

        await SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
      }
    }

    private static async Task SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
      Console.WriteLine("Seeding new platforms");
      foreach(var plat in platforms)
      {
        if (!await repo.ExternalPlatformExists(plat.ExternalId))
        {
          await repo.CreatePlatform(plat);
        }
        await repo.SaveChanges();
      }
    }
  }
}
