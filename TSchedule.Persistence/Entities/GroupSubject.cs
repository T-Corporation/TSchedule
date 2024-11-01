using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSchedule.Persistence.Entities;

public class GroupSubject
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Group))]
    public int GroupId { get; set; }
    
    [ForeignKey(nameof(Subject))]
    public int SubjectId { get; set; }

    // Навигационные свойства
    public Group Group { get; set; } = null!;
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Subject Subject { get; set; } = null!;
}
