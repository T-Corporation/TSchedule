using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Extensions;

namespace TSchedule.Persistence.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasData
        (
            Lessons.First,
            Lessons.Second,
            Lessons.Third,
            Lessons.Fourth,
            Lessons.Fifth,
            Lessons.Sixth
        );
    }
}
