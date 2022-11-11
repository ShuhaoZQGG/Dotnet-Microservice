using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandService.Models
{
  public class Command
  {
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string HowTo { get; set; }
    [Required]
    public string CommandLine { get; set; }
    [ForeignKey("Platforms")]
    [Required]
    public int PlatformId { get; set; }
    public Platform Platform { get; set; }
  }
}
