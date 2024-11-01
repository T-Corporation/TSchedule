using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Configurations;

public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> builder)
    {
        builder.HasIndex(t => t.UserName)
            .IsUnique();

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.HasIndex(t => t.PhoneNumber)
            .IsUnique();
    }
}
