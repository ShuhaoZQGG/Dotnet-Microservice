using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlatformService.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
    
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      Console.WriteLine("Creating Table Platforms");
      // add your own configuration here
      modelBuilder.Entity<Platform>().ToTable("Platforms");
    }

    public DbSet<Platform> Platforms { get; set; }
  }
}
