using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

[Table("Classrooms", Schema = "School")]
public class Classroom
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string RoomNumber { get; set; } = string.Empty;

    [StringLength(100)]
    public string Type { get; set; } = string.Empty;
}
