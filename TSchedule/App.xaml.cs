using System.Windows;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Repositories;
using TSchedule.Persistence.Services;
using TSchedule.Views;

namespace TSchedule;

public partial class App
{
    private static IUsersRepository TeachersRepository { get; } = new UsersRepository();
    public static IUsersService TeachersService { get; } = new UsersService(TeachersRepository);

    public static IUser? User { get; set; }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        WindowManager.CreateWindow<MainWindow>();
    }
}
