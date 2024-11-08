using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Common.IconKeys;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Views;
using TSchedule.Views.Pages.MainWindow;

namespace TSchedule.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private NavigationViewItem? _navigationItem;

    private readonly Lazy<IUsersService> _usersService = new(ServiceManager.Default.GetRequiredService<IUsersService>);
    public Frame NavigationFrame { get; set; }

    public string UserName => _usersService.Value.GetUserName();
    public string FullName => _usersService.Value.GetFullName();
    public string Initials => FullName.ToInitials();

    public ObservableCollection<NavigationViewItem> NavigationItems { get; set; } = [];

    public MainWindowViewModel(Frame navigationFrame)
    {
        NavigationFrame = navigationFrame;
        NavigationFrame.Navigate(new HomePage());
        InitializeNavigationItems();
        NavigationItem = NavigationItems[0];
    }

    private void InitializeNavigationItems()
    {
        NavigationItems.Add(CreateNavigationItem("Главная", PageCode.Home, SegoeFluentIcons.Home));

        var subjectsItem = CreateNavigationItem("Предметы", PageCode.Subjects, SegoeFluentIcons.Bookmarks);
        var specialtiesItem = CreateNavigationItem("Специальности", PageCode.Specialties, FluentSystemIcons.Ruler_20_Regular);
        var groupsItem = CreateNavigationItem("Группы", PageCode.Groups, FluentSystemIcons.PeopleTeam_20_Regular);
        var classroomsItem = CreateNavigationItem("Аудитории", PageCode.Classrooms, FluentSystemIcons.Class_20_Regular);
        var notificationsItem = new NavigationViewItem { Content = "Уведомления", Icon = new FontIcon(SegoeFluentIcons.Ringer) };
        var teachersItem = CreateNavigationItem("Преподаватели", PageCode.Teachers, FluentSystemIcons.Clipboard_20_Regular);
        var scheduleItem = new NavigationViewItem { Content = "Расписание", Icon = new FontIcon(SegoeFluentIcons.Calendar) };

        switch (ServiceManager.Default.GetRequiredService<IUsersService>().GetRole())
        {
            case Role.Гость:
                notificationsItem.Tag = PageCode.Announcements;
                scheduleItem.Tag = PageCode.Schedule;
                AddGuestNavigationItems(scheduleItem, classroomsItem, specialtiesItem, subjectsItem, groupsItem, teachersItem, notificationsItem);
                break;

            case Role.Преподаватель:
                AddTeacherNavigationItems(scheduleItem, classroomsItem, specialtiesItem, subjectsItem, groupsItem, teachersItem, notificationsItem);
                break;

            case Role.Администратор:
                AddAdministratorNavigationItems(scheduleItem, notificationsItem);
                break;
        }
    }

    private NavigationViewItem CreateNavigationItem(string content, PageCode pageCode, FontIconData iconData)
    {
        return new NavigationViewItem
        {
            Tag = pageCode,
            Content = content,
            Icon = new FontIcon(iconData)
        };
    }

    private void AddGuestNavigationItems(params NavigationViewItem[] items)
    {
        foreach (var item in items)
            NavigationItems.Add(item);
    }

    private void AddTeacherNavigationItems(params NavigationViewItem[] items)
    {
        foreach (var item in items)
            NavigationItems.Add(item);
        
        items[^1].MenuItems.Add(CreateNavigationItem("Просмотр уведомлений", PageCode.Announcements, SegoeFluentIcons.View));
        items[^1].MenuItems.Add(CreateNavigationItem("Создать уведомление", PageCode.CreateAnnouncements, SegoeFluentIcons.Add));
    }

    private void AddAdministratorNavigationItems(NavigationViewItem scheduleItem, NavigationViewItem notificationsItem)
    {
        notificationsItem.MenuItems.Add(CreateNavigationItem("Просмотр уведомлений", PageCode.Announcements, SegoeFluentIcons.View));
        notificationsItem.MenuItems.Add(CreateNavigationItem("Регистрация уведомлений", PageCode.RegisterAnnouncements, SegoeFluentIcons.CheckMark));
        NavigationItems.Add(notificationsItem);

        scheduleItem.MenuItems.Add(CreateNavigationItem("Добавление расписания", PageCode.ScheduleManagement, SegoeFluentIcons.Add));
        NavigationItems.Add(scheduleItem);

        var managementItem = new NavigationViewItem
        {
            Tag = PageCode.None,
            Content = "Управление",
            Icon = new FontIcon(FluentSystemIcons.Toolbox_20_Regular),
            MenuItems = {
                CreateNavigationItem("Аудитории", PageCode.ClassroomsManagement, FluentSystemIcons.Class_20_Regular),
                CreateNavigationItem("Специальности", PageCode.SpecialtiesManagement, FluentSystemIcons.Ruler_20_Regular),
                CreateNavigationItem("Предметы", PageCode.SubjectsManagement, FluentSystemIcons.Book_20_Regular),
                CreateNavigationItem("Группы", PageCode.GroupsManagement, FluentSystemIcons.PeopleTeamToolbox_20_Regular),
                CreateNavigationItem("Преподаватели", PageCode.TeachersManagement, FluentSystemIcons.Clipboard_20_Regular)
            }
        };
        NavigationItems.Add(managementItem);
    }

    partial void OnNavigationItemChanged(NavigationViewItem? value)
    {
        if (value is not null
            && value.Tag is PageCode pageCode
            && NavigationFrame.Content?.ToPageCode() != pageCode)
            NavigationFrame.Navigate(pageCode.ToPage());
    }

    [RelayCommand]
    public void GoBack() => NavigationFrame.GoBack();

    [RelayCommand]
    public void GoForward() => NavigationFrame.GoForward();

    [RelayCommand]
    private void Refresh() => NavigationFrame.Refresh();

    [RelayCommand]
    private void GoToParameters() => NavigateTo(new SettingsPage());

    public void NavigateTo(Page page) => NavigationFrame.Navigate(page);

    [RelayCommand]
    private void Logout()
    {
        var usersService = _usersService.Value;
        usersService.Logout();
        PreferencesManager.Default.SetRole("Гость");
        PreferencesManager.Default.SetUserGuid(Guid.Empty);
        PreferencesManager.Default.SetLoggedIn(false);
        PreferencesManager.Default.Save();

        WindowManager.Default.CreateWindow<StartWindow>();
        WindowManager.Default.CloseWindow<MainWindow>();
    }
}
