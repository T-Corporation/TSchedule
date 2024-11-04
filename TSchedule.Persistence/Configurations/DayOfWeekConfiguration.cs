using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Extensions;

namespace TSchedule.Persistence.Configurations;

public class DayOfWeekConfiguration : IEntityTypeConfiguration<WeekDay>
{
    public void Configure(EntityTypeBuilder<WeekDay> builder)
    {
        builder.HasData(
            WeekDays.Monday,
            WeekDays.Tuesday,
            WeekDays.Wednesday,
            WeekDays.Thursday,
            WeekDays.Friday,
            WeekDays.Saturday,
            WeekDays.Sunday
        );
    }
}
