using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSchedule.Persistence.Entities;

[Table("Products", Schema = "Commerce")]
public class Product
{
    [Key]
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, MinimumLength = 8)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, MinimumLength = 8)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
