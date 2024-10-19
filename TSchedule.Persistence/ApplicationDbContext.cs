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
        optionsBuilder.UseSqlServer("Server=LAPTOP-AQTL78HJ;Database=TSchedule;Trusted_Connection=True;TrustServerCertificate=True");
    }

    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Workload> Workloads { get; set; }
}
