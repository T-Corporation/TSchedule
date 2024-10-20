using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TSchedule.Managers;
using TSchedule.Views;

namespace TSchedule.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [RelayCommand]
    private void GoToLoginWindow()
    {
        WindowManager.Default.CreateWindow<LoginWindow>(true, WindowManager.Default.GetWindow<MainWindow>());
    }
}
