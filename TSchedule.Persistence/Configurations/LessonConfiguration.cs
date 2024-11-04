using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasData
        (
            new Lesson
            {
                Id = 1,
                StartTime = TimeOnly.Parse("8:30"),
                EndTime = TimeOnly.Parse("10:05")
            },
            new Lesson
            {
                Id = 2,
                StartTime = TimeOnly.Parse("10:15"),
                EndTime = TimeOnly.Parse("11:50")
            },
            new Lesson
            {
                Id = 3,
                StartTime = TimeOnly.Parse("12:30"),
                EndTime = TimeOnly.Parse("14:05")
            },
            new Lesson
            {
                Id = 4,
                StartTime = TimeOnly.Parse("14:15"),
                EndTime = TimeOnly.Parse("15:50")
            },
            new Lesson
            {
                Id = 5,
                StartTime = TimeOnly.Parse("16:00"),
                EndTime = TimeOnly.Parse("17:35")
            },
            new Lesson
            {
                Id = 6,
                StartTime = TimeOnly.Parse("17:45"),
                EndTime = TimeOnly.Parse("19:20")
            }
        );
    }
}
