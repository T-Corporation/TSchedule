using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Views.Pages.MainWindow;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class HomePageViewModel(Frame navigationFrame) : ObservableObject
{
    private readonly Lazy<IUsersService> _usersService = new(ServiceManager.Default.GetRequiredService<IUsersService>);

    public IUsersService UsersService => _usersService.Value;

    public string FullName => UsersService.GetUserFullName();

    [RelayCommand]
    private void GoToAnnouncements() => navigationFrame.Navigate(new AnnouncementsPage());

    [RelayCommand]
    private void GoToSchedule() => navigationFrame.Navigate(new SchedulePage());

    [RelayCommand]
    private void GoToClassrooms() => navigationFrame.Navigate(new ClassroomsPage());
}
