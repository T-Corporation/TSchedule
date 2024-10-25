using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IAnnouncementsService : IService
{
    Task<Announcement> GetAnnouncementById(int id);
    Task<IEnumerable<Announcement>> GetAllAnnouncements();
    Task<IEnumerable<Announcement>> GetAnnouncementsByTeacherId(Guid id);

    Task<bool> RegisterAnnouncement(Announcement announcement);

    Task<bool> AddAnnouncement(Announcement announcement);
    Task<bool> UpdateAnnouncement(Announcement announcement);
    Task<bool> RemoveAnnouncement(int id);
}
