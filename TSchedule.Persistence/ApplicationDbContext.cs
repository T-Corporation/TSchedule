using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Managers;

namespace TSchedule.Persistence;

/// <summary>
/// Ключевой класс, который будет работать с БД
/// </summary>
public class ApplicationDbContext : DbContext
{
    // Нужно использовать, удостоверившись, что БД пустая
    // public ApplicationDbContext() => Database.EnsureCreated();
	
	/// <summary>
	/// Совершает прогрев БД простым запросом
	/// </summary>
	public async Task WarmUpAsync() => await Teachers.AnyAsync();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConnectionManager.Default.GetConnectionString());

    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<RegisteredAnnouncement> RegisteredAnnouncements { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Workload> Workloads { get; set; }
}
