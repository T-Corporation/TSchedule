using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.HasIndex(t => t.PhoneNumber)
            .IsUnique();

        builder.HasMany(t => t.PreferredTimes)
            .WithOne(tpt => tpt.Teacher)
            .HasForeignKey(tpt => tpt.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
