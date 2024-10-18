using Microsoft.EntityFrameworkCore;

namespace TSchedule.Persistence;

/// <summary>
/// Ключевой класс, который будет работать с БД
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() => Database.EnsureCreated();
}
