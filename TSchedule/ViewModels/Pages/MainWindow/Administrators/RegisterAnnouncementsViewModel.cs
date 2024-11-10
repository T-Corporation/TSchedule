using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class RegisterAnnouncementsViewModel : ObservableObject
{
    private const string AccessViolationMessage = "Ваш аккаунт не обладает достаточными правами для выполнения этого действия";

    [ObservableProperty]
    private ObservableCollection<AnnouncementModel> _announcements = [];

    private static readonly IAnnouncementsService _announcementsService
        = ServiceManager.Default.GetRequiredService<IAnnouncementsService>();

    private RegisterAnnouncementsViewModel(IEnumerable<Announcement> announcements)
    {
        foreach (var announcement in announcements)
            Announcements.Add(announcement.ToModel());
    }

    public static async Task<RegisterAnnouncementsViewModel> CreateInstanceAsync()
        => new(await _announcementsService.GetAllAnnouncements());

    [RelayCommand]
    private void UpdateRegistration(AnnouncementModel? announcementModel)
    {
        if (announcementModel is null) return;

        var currentUserRole = ServiceManager.Default.GetRequiredService<IUsersService>().GetRole();

        if (currentUserRole is not Role.Администратор)
            throw new AccessViolationException(AccessViolationMessage);

        var foundAnnouncement = Announcements.First(a => a.Id == announcementModel.Id);
        foundAnnouncement.UpdatedAt = DateTime.Now;
        foundAnnouncement.IsRegistered = announcementModel.IsRegistered;

        if (!announcementModel.IsRegistered)
            _announcementsService.UnregisterAnnouncement(announcementModel.ToEntity());
        else
            _announcementsService.RegisterAnnouncement(announcementModel.ToEntity());
    }

    [RelayCommand]
    private void Delete(AnnouncementModel announcementModel)
    {
        if (WindowManager.ShowMessageBox(
            "Вы уверены, что хотите удалить это уведомление?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question) is not MessageBoxResult.Yes) return;

        _announcementsService.RemoveAnnouncement(announcementModel.Id);
        Announcements.Remove(Announcements.First(a => a.Id == announcementModel.Id));
    }
}
