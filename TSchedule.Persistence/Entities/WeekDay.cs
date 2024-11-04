using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSchedule.Persistence.Entities;

[Table("WeekDays", Schema = "Academic")]
public class WeekDay : IComparable<WeekDay>
{
    [Key] public int Id { get; set; }

    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = string.Empty;

    public override int GetHashCode()
        => Id.GetHashCode();

    public override bool Equals(object? obj)
        => obj is WeekDay wd && wd.GetHashCode() == GetHashCode();

    public int CompareTo(WeekDay? other)
    {
        if (other is null) return 1; // Или 0 в зависимости от вашей логики
        return Id.CompareTo(other.Id); // Сравнение по Id или любому другому критерию
    }
}
