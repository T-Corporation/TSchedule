using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Managers;
using WeekDay = TSchedule.Persistence.Entities.WeekDay;

namespace TSchedule.Persistence;

/// <summary>
/// Ключевой класс, который будет работать с БД
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
	/// Совершает прогрев БД простым запросом
	/// </summary>
	public async Task WarmUpAsync() => await Teachers.AnyAsync();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConnectionManager.Default.GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<WeekDay> DaysOfWeek { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupSubject> GroupSubjects { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TeacherPreferredTime> TeacherPreferredTimes { get; set; }
}
