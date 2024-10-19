using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;

namespace TSchedule;

/// <summary>
/// Ключевой класс, который будет работать с БД
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=LAPTOP-AQTL78HJ;Database=TSchedule;Trusted_Connection=True;TrustServerCertificate=True");
    }

    // Здесь будут таблицы
    public DbSet<Teacher> Teachers { get; set; }
}
