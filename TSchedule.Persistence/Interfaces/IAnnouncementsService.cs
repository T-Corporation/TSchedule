using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IAnnouncementsService : IService
{
    Task<Announcement> GetAnnouncementById(int id);
    Task<IEnumerable<Announcement>> GetAllAnnouncements();
    Task<IEnumerable<Announcement>> GetAnnouncementsByTeacherId(Guid id);

    Task<Announcement> GetRegisteredAnnouncementById(int id);
    Task<IEnumerable<Announcement>> GetAllRegisteredAnnouncements();

    Task<bool> IsAnnouncementRegistered(int id);
    Task RegisterAnnouncement(Announcement announcement);
    Task UnregisterAnnouncement(Announcement announcement);

    Task AddAnnouncement(Announcement announcement);
    Task UpdateAnnouncement(Announcement announcement);
    Task RemoveAnnouncement(int id);
}
