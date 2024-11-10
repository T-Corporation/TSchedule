using TSchedule.Managers;
using TSchedule.Persistence;
using TSchedule.Persistence.Services;
using TSchedule.Persistence.Managers;
using TSchedule.Views;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Repositories;
using TSchedule.Persistence.Enums;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using System.Text;
using Windows.UI.ViewManagement;
using OfficeOpenXml;
using iNKORE.UI.WPF.Helpers;

namespace TSchedule;

public partial class App
{
    #pragma warning disable CA1416 // Проверка совместимости платформы
    public static readonly UISettings UISettings = new();

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            UISettings.ColorValuesChanged += OnColorValuesChanged;
            CustomizationManager.Default.LoadStyleFromSettings()
                .ApplyThemeFromPreferences(UISettings);

            ServiceManager.Default
                .AddSingleton<IUsersService, UsersService>(() => new UsersService(new UsersRepository(PreferencesManager.Default.GetConnectionString())))
                .AddSingleton<IScheduleService, ScheduleService>(() => new ScheduleService(new ScheduleRepository(PreferencesManager.Default.GetConnectionString())))
                .AddSingleton<IAnnouncementsService, AnnouncementsService>(() => new AnnouncementsService(PreferencesManager.Default.GetConnectionString()))
                .AddSingleton<IGroupsService, GroupsService>(() => new GroupsService(PreferencesManager.Default.GetConnectionString()))
                .AddSingleton<IWeekDaysService, WeekDaysService>(() => new WeekDaysService(PreferencesManager.Default.GetConnectionString()))
                .AddSingleton<ITeachersService, TeachersService>(() => new TeachersService(PreferencesManager.Default.GetConnectionString()))
                .AddSingleton<IClassroomsService, ClassroomsService>(() => new ClassroomsService(PreferencesManager.Default.GetConnectionString()))
                .AddSingleton<ISpecialtiesService, SpecialtiesService>(() => new SpecialtiesService(PreferencesManager.Default.GetConnectionString()))
                .AddSingleton<ISubjectsService, SubjectsService>(() => new SubjectsService(PreferencesManager.Default.GetConnectionString()));

            if (Environment.GetCommandLineArgs().Contains("User-Connection"))
            {
                if (OSVersionHelper.IsWindows10OrGreater && PreferencesManager.Default.IsModernUIEnabled())
                    WindowManager.Default.CreateWindow<UserConnectionWindow>(showDialog: true);
                else
                    WindowManager.Default.CreateWindow<UserConnectionWindowWin7>(showDialog: true);
                return;
            }

            await using ApplicationDbContext context = new(PreferencesManager.Default.GetConnectionString());
            await context.WarmUpAsync();
            await InitializeEntry();
        }
        catch (SqlException)
        {
            if (OSVersionHelper.IsWindows10OrGreater && PreferencesManager.Default.IsModernUIEnabled())
                WindowManager.Default.CreateWindow<StartWindow>();
            else
                WindowManager.Default.CreateWindow<StartWindowWin7>();

            if (WindowManager.ShowMessageBox(
                "Отсутствует подключение к интернету или указана неверная строка подключения.",
                "Ошибка, перезагрузить приложение?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error) is MessageBoxResult.Yes)
                Restart();
        }
    }

    /// <summary>
    /// Инициализирует вход в приложение
    /// </summary>
    /// <returns>Задача</returns>
    private static async Task InitializeEntry()
    {
        var isLoggedIn = PreferencesManager.Default.IsLoggedIn();

        if (!isLoggedIn)
        {
            if (OSVersionHelper.IsWindows10OrGreater && PreferencesManager.Default.IsModernUIEnabled())
                WindowManager.Default.CreateWindow<StartWindow>();
            else
                WindowManager.Default.CreateWindow<StartWindowWin7>();
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

        if (OSVersionHelper.IsWindows10OrGreater && PreferencesManager.Default.IsModernUIEnabled())
            WindowManager.Default.CreateWindow<MainWindow>();
        else
            WindowManager.Default.CreateWindow<MainWindowWin7>();
    }

    private void OnColorValuesChanged(UISettings sender, object args)
        => Dispatcher.Invoke(() => CustomizationManager.Default.ApplyThemeFromPreferences(UISettings));

    private void OnDispatcherUnhandledException(object _, DispatcherUnhandledExceptionEventArgs e)
    {
        HandleException(e.Exception);
        e.Handled = true;
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
            HandleException(ex);
    }

    private void HandleException(Exception exception)
    {
        var fullMessage = BuildExceptionMessage(exception);
        Debug.WriteLine(fullMessage);

        // Проверка, является ли исключение SqlException
        if (exception is SqlException sqlEx)
        {
            if (WindowManager.ShowMessageBox(
                $"""
                Сообщение: {sqlEx.Message}
                """,
                "Ошибка работы БД. Перезапустить приложение?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error) is MessageBoxResult.Yes)
                Restart();
            return;
        }
        
        // Логирование или обработка других исключений
        if (WindowManager.ShowMessageBox(
            $"""
            Вызванные исключения:
            {fullMessage}
            """,
            "Неожиданная ошибка. Закрыть приложение?",
            MessageBoxButton.YesNo,
            MessageBoxImage.Error) is MessageBoxResult.Yes)
            Shutdown();
    }

    private static string BuildExceptionMessage(Exception exception)
    {
        StringBuilder messageBuilder = new();
        var currentException = exception;
        int level = 1;

        while (currentException is not null)
        {
            messageBuilder.AppendLine($"(Уровень {level++}) {currentException.GetType().Name}: \"{currentException.Message}\"");
            currentException = currentException.InnerException;
        }

        return messageBuilder.ToString();
    }

    public static void Restart()
    {
        var exePath = Process.GetCurrentProcess().MainModule?.FileName;
        if (!string.IsNullOrEmpty(exePath)) Process.Start(exePath);
        Current.Shutdown();
    }

    protected override void OnActivated(EventArgs e) => ServiceManager.Default.Dispose();

    protected override void OnExit(ExitEventArgs e)
    {
        MusicManager.Default.SoundPlayer.Stop();
        MusicManager.Default.SoundPlayer.Dispose();
    }
}
