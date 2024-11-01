using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Configurations;

public class TeacherPreferredTimeConfiguration : IEntityTypeConfiguration<TeacherPreferredTime>
{
    public void Configure(EntityTypeBuilder<TeacherPreferredTime> builder)
    {
        builder.HasIndex(pt => new
        {
            pt.TeacherId,
            pt.DayOfWeekId
        }).IsUnique();
    }
}
