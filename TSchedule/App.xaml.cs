using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence;
using TSchedule.Persistence.Services;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Repositories;
using TSchedule.Views;

namespace TSchedule;

public partial class App
{
    private async void Application_Startup(object sender, StartupEventArgs e)
    {
		await using ApplicationDbContext context = new();
		await context.WarmUpAsync();

        UserPreferencesManager.SetupTheme();

        ServiceManager.Default.AddSingleton<UsersRepository, UsersService>();

        WindowManager.Default.CreateWindow<StartWindow>();
    }

    protected override void OnActivated(EventArgs e)
    {
        ServiceManager.Default.Dispose();
    }
}
