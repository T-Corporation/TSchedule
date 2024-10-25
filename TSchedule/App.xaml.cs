using TSchedule.Managers;
using TSchedule.Persistence;
using TSchedule.Persistence.Services;
using TSchedule.Persistence.Managers;
using TSchedule.Views;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Repositories;
using TSchedule.Persistence.Enums;

namespace TSchedule;

public partial class App
{
    private async void Application_Startup(object sender, System.Windows.StartupEventArgs e)
    {
		#if DEBUG
        PreferencesManager.Default.PrintValues();
        #endif

        await using ApplicationDbContext context = new();
		await context.WarmUpAsync();
        /*await context.Administrators.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            UserName = "roman",
            Email = "roman@mail.ru",
            PhoneNumber = "70000000000",
            FullName = "Шибалов Роман Николаевич",
            PasswordHash = PasswordManager.Default.HashPassword("Qwerty123!")
        });
        await context.SaveChangesAsync();*/

        CustomizationManager.Default
            .LoadStyleFromSettings()
            .ApplyCurrentTheme();

        ServiceManager.Default
            .AddSingleton<IUsersService, UsersService>(() => new UsersService(new UsersRepository()))
            .AddSingleton<ITeachersService, TeachersService>();

        var isLoggedIn = PreferencesManager.Default.IsLoggedIn();

        if (!isLoggedIn)
        {
            WindowManager.Default.CreateWindow<StartWindow>();
            return;
        }

        var id = PreferencesManager.Default.GetUserGuid();
        var role = PreferencesManager.Default.GetRole();

        var usersService = ServiceManager.Default.GetRequiredService<IUsersService>();
        await usersService.AuthenticateById(id, role switch
        {
            "Преподаватель" => Role.Преподаватель,
            "Администратор" => Role.Администратор,
            _ => Role.Гость
        });
        
        WindowManager.Default.CreateWindow<MainWindow>();
    }

    protected override void OnActivated(EventArgs e)
    {
        ServiceManager.Default.Dispose();
    }
}
