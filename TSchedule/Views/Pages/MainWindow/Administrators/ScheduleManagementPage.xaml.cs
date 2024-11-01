using System.Windows;
using TSchedule.Persistence.Models;
using TSchedule.ViewModels.Pages.MainWindow.Administrators;

namespace TSchedule.Views.Pages.MainWindow.Administrators;

public partial class ScheduleManagementPage
{
    private readonly GroupModel _selectedGroup;

    public ScheduleManagementPage(GroupModel selectedGroup)
    {
        InitializeComponent();
        _selectedGroup = selectedGroup;
    }

    private async void Page_Loaded(object _, RoutedEventArgs __)
        => DataContext = await ScheduleManagementViewModel.CreateInstanceAsync(_selectedGroup!, EditFlyout, TabControl);
}
