using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Teachers", Schema = "School")]
public class Teacher : ApplicationUser
{
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }

    [ForeignKey("Classroom")]
    public int ClassroomId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Classroom? Classroom { get; set; }

    public ICollection<TeacherPreferredTime> PreferredTimes { get; set; } = [];

    [ForeignKey("Subject")]
    public int SubjectId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Subject? Subject { get; set; }

    [NotMapped]
    public override Role Role => Role.Преподаватель;

    public TeacherModel ToModel()
        => new()
        {
            Id = Id,
            IsDeleted = IsDeleted,
            UserName = UserName,
            PasswordHash = PasswordHash,
            PhoneNumber = PhoneNumber,
            Email = Email,
            FullName = FullName,
            DateOfBirth = DateOfBirth,
            PreferredTimes = [.. PreferredTimes.Select(pt => pt.ToModel())],
            Subject = Subject?.ToModel(),
            Classroom = Classroom?.ToModel()
        };
}
