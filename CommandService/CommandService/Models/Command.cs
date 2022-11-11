using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandService.Models
{
  public class Command
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string HowTo { get; set; }
    [Required]
    public string CommandLine { get; set; }
    [ForeignKey("Platforms")]
    [Required]
    public Guid PlatformId { get; set; }
    public Platform Platform { get; set; }
  }
}
