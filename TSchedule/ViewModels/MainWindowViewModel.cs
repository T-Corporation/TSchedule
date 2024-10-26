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

    public bool IsAuthenticated { get; }

    partial void OnNavigationItemChanged(NavigationViewItem? value)
    {
        // Проверка, что вызов не будет выполнен повторно при срабатывании OnNavigationItemChanged
        if (value is not null
            && value.Tag is PageCode pageCode
            && NavigationFrame.Content?.ToPageCode() != pageCode)
        {
            NavigationFrame.Navigate(pageCode.ToPage());
        }
    }

    private readonly Lazy<IUsersService> lazyUsersService = new(ServiceManager.Default.GetRequiredService<IUsersService>);

    public Frame NavigationFrame { get; set; }

    public string UserName => lazyUsersService.Value.GetUserName();

    public string FullName => lazyUsersService.Value.GetUserFullName();

    public string Initials => FullName.ToInitials();

    public ObservableCollection<NavigationViewItem> NavigationItems { get; set; } = [];

    public MainWindowViewModel(Frame navigationFrame)
    {
        NavigationFrame = navigationFrame;
        NavigationFrame.Navigate(new HomePage());

        NavigationItems.Add(new NavigationViewItem()
        {
            Tag = PageCode.Home,
            Content = "Главная",
            Icon = new FontIcon(SegoeFluentIcons.Home)
        });
        NavigationItems.Add(new NavigationViewItem
        {
            Content = "Предметы",
            Tag = PageCode.Subjects,
            Icon = new FontIcon(SegoeFluentIcons.Bookmarks)
        });
        NavigationItems.Add(new NavigationViewItem
        {
            Content = "Группы",
            Tag = PageCode.Groups,
            Icon = new FontIcon(FluentSystemIcons.PeopleTeam_20_Regular)
        });

        var notificationsItem = new NavigationViewItem
        {
            Content = "Уведомления",
            Icon = new FontIcon(SegoeFluentIcons.Ringer)
        };

        var usersService = ServiceManager.Default.GetRequiredService<IUsersService>();
        IsAuthenticated = usersService.IsAuthenticated();

        switch (usersService.GetRole())
        {
            case Role.Гость:
                notificationsItem.Tag = PageCode.Announcements;
                NavigationItems.Add(notificationsItem);
                break;

            case Role.Администратор:
                // Для администраторов "Уведомления" как меню с подэлементами
                notificationsItem.MenuItems.Add(new NavigationViewItem
                {
                    Content = "Просмотр уведомлений",
                    Tag = PageCode.Announcements,
                    Icon = new FontIcon(SegoeFluentIcons.View)
                });
                notificationsItem.MenuItems.Add(new NavigationViewItem
                {
                    Content = "Регистрация уведомлений",
                    Tag = PageCode.RegisterAnnouncements,
                    Icon = new FontIcon(SegoeFluentIcons.CheckMark)
                });
                NavigationItems.Add(notificationsItem);

                var scheduleItem = new NavigationViewItem()
                {
                    Tag = PageCode.Schedule,
                    Content = "Расписание",
                    Icon = new FontIcon(SegoeFluentIcons.Calendar)
                };
                scheduleItem.MenuItems.Add(new NavigationViewItem()
                {
                    Content = "Добавление расписания",
                    Tag = PageCode.CreateSchedule,
                    Icon = new FontIcon(SegoeFluentIcons.Add)
                });
                scheduleItem.MenuItems.Add(new NavigationViewItem()
                {
                    Content = "Правка расписания",
                    Tag = PageCode.EditSchedule,
                    Icon = new FontIcon(SegoeFluentIcons.Edit)
                });
                NavigationItems.Add(scheduleItem);

                // Management Section
                var managementItem = new NavigationViewItem()
                {
                    Content = "Управление",
                    Icon = new FontIcon(FluentSystemIcons.Toolbox_20_Regular)
                };
                managementItem.MenuItems.Add(new NavigationViewItem()
                {
                    Content = "Аудитории",
                    Tag = PageCode.ClassroomsManagement,
                    Icon = new FontIcon(FluentSystemIcons.Class_20_Regular)
                });
                managementItem.MenuItems.Add(new NavigationViewItem()
                {
                    Content = "Предметы",
                    Tag = PageCode.SubjectsManagement,
                    Icon = new FontIcon(FluentSystemIcons.Book_20_Regular)
                });
                managementItem.MenuItems.Add(new NavigationViewItem()
                {
                    Content = "Группы",
                    Tag = PageCode.GroupsManagement,
                    Icon = new FontIcon(FluentSystemIcons.PeopleTeamToolbox_20_Regular)
                });
                managementItem.MenuItems.Add(new NavigationViewItem()
                {
                    Content = "Преподаватели",
                    Tag = PageCode.TeachersManagement,
                    Icon = new FontIcon(FluentSystemIcons.Clipboard_20_Regular)
                });
                NavigationItems.Add(managementItem);
                break;

            case Role.Преподаватель:
                // Для преподавателей "Уведомления" как меню с подэлементами
                notificationsItem.MenuItems.Add(new NavigationViewItem
                {
                    Tag = PageCode.Announcements,
                    Content = "Просмотр уведомлений",
                    Icon = new FontIcon(SegoeFluentIcons.View)
                });
                notificationsItem.MenuItems.Add(new NavigationViewItem
                {
                    Content = "Создание уведомлений",
                    Tag = PageCode.CreateAnnouncements,
                    Icon = new FontIcon(SegoeFluentIcons.Add)
                });
                NavigationItems.Add(notificationsItem);

                NavigationItems.Add(new NavigationViewItem
                {
                    Tag = PageCode.MyGroup,
                    Content = "Моя группа",
                    Icon = new FontIcon(FluentSystemIcons.PeopleTeam_20_Filled)
                });
                NavigationItems.Add(new NavigationViewItem
                {
                    Tag = PageCode.CreateAnnouncements,
                    Content = "Добавление уведомлений",
                    Icon = new FontIcon(SegoeFluentIcons.Add)
                });
                break;
        }

        NavigationItem = NavigationItems[0];
    }

    [RelayCommand]
    public void GoBack()
    {
        NavigationFrame.GoBack();
    }

    [RelayCommand]
    public void GoForward()
    {
        NavigationFrame.GoForward();
    }

    [RelayCommand]
    private void GoToProfile()
    {
        NavigateTo(new ProfilePage());
    }

    [RelayCommand]
    private void GoToParameters()
    {
        NavigateTo(new SettingsPage());
    }

    public void NavigateTo(Page page)
    {
        NavigationFrame.Navigate(page);
    }

    [RelayCommand]
    private void Logout()
    {
        var usersService = lazyUsersService.Value;
        usersService.Logout();
        WindowManager.Default.CreateWindow<StartWindow>();
        WindowManager.Default.CloseWindow<MainWindow>();
    }
}
