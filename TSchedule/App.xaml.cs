using System.Windows;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Repositories;
using TSchedule.Persistence.Services;
using TSchedule.Views;

namespace TSchedule;

public partial class App
{
    public static ITeachersService TeachersService { get; }

    public App()
    {
        TeachersService = new TeachersService(
            new TeachersRepository());
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        WindowManager.CreateWindow<MainWindow>();
    }
}
