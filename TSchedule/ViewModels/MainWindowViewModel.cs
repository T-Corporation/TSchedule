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
    private NavigationViewItem _navigationItem;

    [ObservableProperty]
    private string _navigationTitle = null!;

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
            Content = "Профиль",
            Tag = PageCode.Profile,
            Icon = new FontIcon(FluentSystemIcons.PersonAccounts_20_Regular)
        });
        NavigationItems.Add(new NavigationViewItem
        {
            Content = "Помощь",
            Tag = PageCode.Help,
            Icon = new FontIcon(SegoeFluentIcons.Help)
        });
        NavigationItems.Add(new NavigationViewItem
        {
            Content = "Группы",
            Tag = PageCode.Groups,
            Icon = new FontIcon(FluentSystemIcons.PeopleTeam_20_Regular)
        });
        NavigationItems.Add(new NavigationViewItem
        {
            Content = "Уведомления",
            Tag = PageCode.Announcements,
            Icon = new FontIcon(SegoeFluentIcons.Ringer)
        });

        var service = ServiceManager.Default.GetRequiredService<IUsersService>();

        switch (service.GetRole())
        {
            case Role.Администратор:
                NavigationItems.Add(new NavigationViewItem
                {
                    Tag = PageCode.CreateSchedule,
                    Content = "Добавление расписания",
                    Icon = new FontIcon(SegoeFluentIcons.Add)
                });
                NavigationItems.Add(new NavigationViewItem
                {
                    Tag = PageCode.EditSchedule,
                    Content = "Правка расписания",
                    Icon = new FontIcon(SegoeFluentIcons.Edit)
                });
                NavigationItems.Add(new NavigationViewItem
                {
                    Content = "Управление аудиториями",
                    Tag = PageCode.ClassroomsManagement,
                    Icon = new FontIcon(FluentSystemIcons.Class_20_Regular)
                });
                NavigationItems.Add(new NavigationViewItem
                {
                    Tag = PageCode.SubjectsManagement,
                    Content = "Управление предметами",
                    Icon = new FontIcon(FluentSystemIcons.Book_20_Regular)
                });
                NavigationItems.Add(new NavigationViewItem
                {
                    Tag = PageCode.GroupsManagement,
                    Content = "Управление группами",
                    Icon = new FontIcon(FluentSystemIcons.PeopleTeamToolbox_20_Regular)
                });
                NavigationItems.Add(new NavigationViewItem
                {
                    Tag = PageCode.TeachersManagement,
                    Content = "Управление преподавателями",
                    Icon = new FontIcon(FluentSystemIcons.Clipboard_20_Regular)
                });
                NavigationItems.Add(new NavigationViewItem
                {
                    Content = "Регистрация уведомлений",
                    Tag = PageCode.RegisterAnnouncements,
                    Icon = new FontIcon(FluentSystemIcons.Checkmark_20_Regular)
                });
                break;

            case Role.Преподаватель:
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
    private void Logout()
    {
        var usersService = lazyUsersService.Value;
        usersService.Logout();
        WindowManager.Default.CreateWindow<StartWindow>();
        WindowManager.Default.CloseWindow<MainWindow>();
    }
}
