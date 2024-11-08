using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class AnnouncementsService(string connectionString) : IAnnouncementsService
{
    public async Task AddAnnouncement(Announcement announcement)
    {
        await using ApplicationDbContext context = new(connectionString);
        await context.AddAsync(announcement);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Announcement>> GetAllAnnouncements()
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Announcements.AsNoTracking()
            .Include(t => t.Teacher)
                .ThenInclude(t => t!.Subject)
            .Include(a => a.Teacher)
                .ThenInclude(t => t!.Classroom)
            .ToListAsync();
    }

    public async Task<IEnumerable<Announcement>> GetAllRegisteredAnnouncements()
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Announcements.AsNoTracking()
            .Include(t => t.Teacher)
                .ThenInclude(t => t!.Subject)
            .Include(a => a.Teacher)
                .ThenInclude(t => t!.Classroom)
            .Where(a => a.IsRegistered)
            .ToListAsync();
    }

    public async Task<Announcement> GetAnnouncementById(int id)
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Announcements.AsNoTracking()
            .Include(a => a.Teacher)
                .ThenInclude(t => t!.Subject)
            .Include(a => a.Teacher)
                .ThenInclude(t => t!.Classroom)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new EntityNotFoundException<Announcement>(nameof(id), id);
    }

    public async Task<IEnumerable<Announcement>> GetAnnouncementsByTeacherId(Guid id)
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Announcements.AsNoTracking()
            .Where(a => a.TeacherId == id)
            .Include(t => t.Teacher)
                .ThenInclude(t => t!.Subject)
            .Include(a => a.Teacher)
                .ThenInclude(t => t!.Classroom)
            .ToListAsync();
    }

    public async Task<Announcement> GetRegisteredAnnouncementById(int id)
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Announcements.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id && a.IsRegistered)
            ?? throw new EntityNotFoundException<Announcement>(nameof(id), id);
    }

    public async Task<bool> IsAnnouncementRegistered(int id)
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Announcements.AsNoTracking()
            .AnyAsync(a => a.Id == id && a.IsRegistered);
    }

    public async Task RegisterAnnouncement(Announcement announcement)
    {
        await using ApplicationDbContext context = new(connectionString);
        var foundAnnouncement = await context.Announcements.FirstOrDefaultAsync(a => a.Id == announcement.Id)
            ?? throw new EntityNotFoundException<Announcement>(nameof(announcement.Id), announcement.Id);

        foundAnnouncement.IsRegistered = true;
        foundAnnouncement.UpdatedAt = DateTime.Now;
        await context.SaveChangesAsync();
    }

    public async Task RemoveAnnouncement(int id)
    {
        await using ApplicationDbContext context = new(connectionString);
        var foundAnnouncement = await context.Announcements.FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new EntityNotFoundException<Announcement>(nameof(id), id);

        context.Announcements.Remove(foundAnnouncement);
        await context.SaveChangesAsync();
    }

    public async Task UnregisterAnnouncement(Announcement announcement)
    {
        await using ApplicationDbContext context = new(connectionString);
        var foundAnnouncement = await context.Announcements.FirstOrDefaultAsync(a => a.Id == announcement.Id && a.IsRegistered)
            ?? throw new EntityNotFoundException<Announcement>(nameof(announcement.Id), announcement.Id);

        foundAnnouncement.IsRegistered = false;
        foundAnnouncement.UpdatedAt = DateTime.Now;
        await context.SaveChangesAsync();
    }

    public async Task UpdateAnnouncement(Announcement announcement)
    {
        await using ApplicationDbContext context = new(connectionString);
        var foundAnnouncement = await context.Announcements.FirstOrDefaultAsync(a => a.Id == announcement.Id)
            ?? throw new EntityNotFoundException<Announcement>(nameof(announcement.Id), announcement.Id);

        foundAnnouncement.UpdatedAt = DateTime.Now;
        foundAnnouncement.Reason = announcement.Reason;
        foundAnnouncement.AbsentTo = announcement.AbsentTo;
        foundAnnouncement.AbsentFrom = announcement.AbsentFrom;
        await context.SaveChangesAsync();
    }
}
