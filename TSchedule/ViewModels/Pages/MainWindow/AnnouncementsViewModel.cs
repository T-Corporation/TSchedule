using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class AnnouncementsViewModel : ObservableObject
{
    private static readonly IAnnouncementsService _announcementsService
        = ServiceManager.Default.GetRequiredService<IAnnouncementsService>();

    [ObservableProperty]
    private ObservableCollection<AnnouncementModel> _announcements = [];

    private AnnouncementsViewModel(IEnumerable<Announcement> announcements)
    {
        foreach (var announcement in announcements)
            Announcements.Add(announcement.ToModel());
    }

    public static async Task<AnnouncementsViewModel> CreateInstanceAsync()
        => new(await _announcementsService.GetAllRegisteredAnnouncements());
}
