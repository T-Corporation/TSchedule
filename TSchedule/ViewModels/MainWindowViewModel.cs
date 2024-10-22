using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Services;

namespace TSchedule.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _userName = ServiceManager.Default.GetRequiredService<UsersService>().GetUserName();
}
