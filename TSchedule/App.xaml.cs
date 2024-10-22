using TSchedule.Managers;
using TSchedule.Persistence;
using TSchedule.Persistence.Services;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Repositories;
using TSchedule.Views;

namespace TSchedule;

public partial class App
{
    private async void Application_Startup(object sender, System.Windows.StartupEventArgs e)
    {
		await using ApplicationDbContext context = new();
		await context.WarmUpAsync();

        CustomizationManager.Default.LoadStyleFromSettings()
            .ApplyCurrentTheme();

        ServiceManager.Default.AddSingleton<UsersRepository, UsersService>();

        var isLoggedIn = PreferencesManager.Default.IsLoggedIn();
        if (isLoggedIn)
            WindowManager.Default.CreateWindow<MainWindow>();
        else
            WindowManager.Default.CreateWindow<StartWindow>();

        #if DEBUG
        PreferencesManager.Default.PrintValues();
        #endif
    }

    protected override void OnActivated(EventArgs e)
    {
        ServiceManager.Default.Dispose();
    }
}
