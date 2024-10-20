using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Repositories;
using TSchedule.Persistence.Services;
using TSchedule.Views;

namespace TSchedule;

public partial class App
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        ServiceManager.Default.AddSingleton<UsersRepository, UsersService>();
        WindowManager.Default.CreateWindow<MainWindow>();
    }

    protected override void OnActivated(EventArgs e)
    {
        ServiceManager.Default.Dispose();
    }
}
