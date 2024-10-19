using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence;

/// <summary>
/// Ключевой класс, который будет работать с БД
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=LAPTOP-AQTL78HJ;User Id=roman;Password=fnaf;TrustServerCertificate=True");
    }

    // Здесь будут таблицы
    public DbSet<Teacher> Teachers { get; set; }
}
