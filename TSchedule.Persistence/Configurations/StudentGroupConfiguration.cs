using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Configurations;

public class StudentGroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasIndex(t => t.Code)
            .IsUnique();
    }
}
