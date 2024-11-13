using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Extensions;

namespace TSchedule.Persistence.Entities;

[Table("Licenses", Schema = "Commerce")]
public class License
{
    [Key]
    [Column(TypeName = "char(19)")]
    [Required(AllowEmptyStrings = false)]
    [StringLength(19, MinimumLength = 19)]
    public string Key { get; set; } = LicenseHelper.EmptyKey;

    [ForeignKey(nameof(Product))]
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, MinimumLength = 8)]
    public string ProductName { get; set; } = string.Empty;

    public Product? Product { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }
}
