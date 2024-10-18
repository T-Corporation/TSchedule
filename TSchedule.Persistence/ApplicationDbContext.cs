using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence;

/// <summary>
/// Ключевой класс, который будет работать с БД
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() => Database.EnsureCreated();

    // Здесь будут таблицы
    public DbSet<Teacher> Teachers { get; set; }
}
