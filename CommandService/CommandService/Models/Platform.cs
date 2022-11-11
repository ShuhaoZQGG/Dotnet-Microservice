using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandService.Models
{
  public class Platform
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string ExternalId { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<Command> Commands { get; set; } = new List<Command>();
  }
}
