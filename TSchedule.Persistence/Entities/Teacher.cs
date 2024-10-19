using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSchedule.Persistence.Entities;

[Table("Teachers", Schema = "School")]
public class Teacher
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Required(AllowEmptyStrings = false)]
    public string FullName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [StringLength(50)]
    public string Classroom { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTimeOffset? PreferredTime { get; set; }
}