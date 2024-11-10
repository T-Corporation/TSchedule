using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Teachers;

public partial class CreateAnnouncementsViewModel : ObservableObject
{
    private static readonly IAnnouncementsService AnnouncementsService
        = ServiceManager.Default.GetRequiredService<IAnnouncementsService>();

    private static Guid TeacherId { get; set; }

    private Flyout AttachedFlyout { get; }

    private FrameworkElement Target { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShowEditFlyoutCommand))]
    private AnnouncementModel? _announcement;

    [ObservableProperty]
    private ObservableCollection<AnnouncementModel> _announcements = [];

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _reason = string.Empty;

    [ObservableProperty]
    private DateTime _absentFromDate = DateTime.Now;

    [ObservableProperty]
    private DateTime _absentFromTime = DateTime.Now;

    [ObservableProperty]
    private DateTime _absentToDate = DateTime.Now;

    [ObservableProperty]
    private DateTime _absentToTime = DateTime.Now.AddHours(1);

    private DateTime AbsentFrom => AbsentFromDate.Date + AbsentFromTime.TimeOfDay;
    private DateTime AbsentTo => AbsentToDate.Date + AbsentToTime.TimeOfDay;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private CreateAnnouncementsViewModel(
        IEnumerable<Announcement> announcements,
        FrameworkElement element,
        Flyout flyout)
    {
        Target = element;
        AttachedFlyout = flyout;

        foreach (var announcement in announcements)
            Announcements.Add(announcement.ToModel());
    }

    public static async Task<CreateAnnouncementsViewModel> CreateInstanceAsync(FrameworkElement element, Flyout flyout)
    {
        var usersService = ServiceManager.Default.GetRequiredService<IUsersService>();

        TeacherId = usersService.GetId();

        return new CreateAnnouncementsViewModel(
            await AnnouncementsService.GetAnnouncementsByTeacherId(TeacherId),
            element,
            flyout);
    }

    [RelayCommand]
    private void ShowFlyout()
    {
        IsEditing = false;
        ErrorMessage = string.Empty;
        Reason = string.Empty;
        AbsentFromDate = DateTime.Now;
        AbsentFromTime = DateTime.Now;
        AbsentToDate = DateTime.Now;
        AbsentToTime = DateTime.Now.AddHours(1);
        AttachedFlyout.ShowAt(Target);
    }

    private bool IsAnnouncementNotEmpty() => Announcement is not null;

    [RelayCommand(CanExecute = nameof(IsAnnouncementNotEmpty))]
    private void ShowEditFlyout()
    {
        IsEditing = true;
        ErrorMessage = string.Empty;
        Reason = Announcement!.Reason;
        AbsentFromDate = Announcement.AbsentFrom.Date;
        AbsentFromTime = DateTime.Today + Announcement.AbsentFrom.TimeOfDay;
        AbsentToDate = Announcement.AbsentTo.Date;
        AbsentToTime = DateTime.Today + Announcement.AbsentTo.TimeOfDay;
        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand]
    private void HideFlyout() => AttachedFlyout.Hide();

    [RelayCommand]
    private async Task AddOrEdit()
    {
        ErrorMessage = string.Empty;
        const string pleaseFillField = "Пожалуйста, заполните поле \"{0}\"";

        if (string.IsNullOrEmpty(Reason))
        {
            ErrorMessage = string.Format(pleaseFillField, "Причина");
            return;
        }

        if (AbsentFromDate > AbsentToDate || (AbsentFromDate < AbsentToTime && AbsentFromTime > AbsentToTime))
        {
            ErrorMessage = "Неверно указан период отсутствия";
            return;
        }

        Announcement announcement = new()
        {
            Reason = Reason,
            AbsentTo = AbsentTo,
            TeacherId = TeacherId,
            AbsentFrom = AbsentFrom
        };

        if (IsEditing)
        {
            if (Announcement is null)
            {
                WindowManager.ShowMessageBox(
                    text: "Не выбрано уведомление",
                    caption: "Ошибка",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error);
                return;
            }

            announcement.Id = Announcement.Id;
            await AnnouncementsService.UpdateAnnouncement(announcement);
            var foundAnnouncement = Announcements.First(a => a.Id == Announcement.Id);
            foundAnnouncement.Reason = announcement.Reason;
            foundAnnouncement.AbsentTo = announcement.AbsentTo;
            foundAnnouncement.AbsentFrom = announcement.AbsentFrom;
            foundAnnouncement.UpdatedAt = DateTime.Now;
            HideFlyout();
            return;
        }

        try
        {
            await AnnouncementsService.AddAnnouncement(announcement);
            Announcements.Add(announcement.ToModel());
            HideFlyout();
        }
        catch (UniqueException ue)
        {
            ErrorMessage = ue.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(IsAnnouncementNotEmpty))]
    private async Task Delete()
    {
        if (WindowManager.ShowMessageBox(
            text: "Вы уверены, что хотите удалить предмет?",
            caption: "Подтверждение",
            button: MessageBoxButton.YesNo,
            icon: MessageBoxImage.Question) is not MessageBoxResult.Yes)
            return;

        await AnnouncementsService.RemoveAnnouncement(Announcement!.Id);
        Announcements.Remove(Announcements.First(a => a.Id == Announcement.Id));
    }
}
