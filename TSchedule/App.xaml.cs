using System.Windows;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Repositories;
using TSchedule.Persistence.Services;
using TSchedule.Views;

namespace TSchedule;

public partial class App
{
    private static ApplicationDbContext ApplicationDbContext { get; } = new ApplicationDbContext();
    private static ITeachersRepository TeachersRepository { get; } = new TeachersRepository(ApplicationDbContext);
    public static ITeachersService TeachersService { get; } = new TeachersService(TeachersRepository);

    public static IUser? User { get; set; }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        WindowManager.CreateWindow<MainWindow>();
    }
}
