using TSchedule.Persistence.Enums;
using TSchedule.Views.Pages.MainWindow.Administrators;
using TSchedule.Views.Pages.MainWindow.Teachers;
using TSchedule.Views.Pages.MainWindow;

namespace TSchedule.Extensions;

public static class PageCodes
{
    /// <summary>
    /// Сопоставляет страницы с кодами
    /// </summary>
    /// <param name="page">Страница</param>
    /// <returns>Код страницы</returns>
    public static PageCode ToPageCode(this object page) => page switch
    {
        ClassroomsManagementPage => PageCode.ClassroomsManagement,
        CreateSchedulePage => PageCode.CreateSchedule,
        EditSchedulePage => PageCode.EditSchedule,
        GroupsManagementPage => PageCode.GroupsManagement,
        RegisterAnnouncementsPage => PageCode.RegisterAnnouncements,
        SubjectsManagementPage => PageCode.SubjectsManagement,
        TeachersManagementPage => PageCode.TeachersManagement,
        CreateAnnouncementPage => PageCode.CreateAnnouncements,
        MyGroupPage => PageCode.MyGroup,
        AnnouncementsPage => PageCode.Announcements,
        ClassroomsPage => PageCode.Classrooms,
        GroupsPage => PageCode.Groups,
        HelpPage => PageCode.Help,
        ProfilePage => PageCode.Profile,
        SchedulePage => PageCode.Schedule,
        SettingsPage => PageCode.Settings,
        SubjectsPage => PageCode.Subjects,
        TeachersPage => PageCode.Teachers,
        _ => PageCode.Home // По умолчанию — главная страница
    };

    /// <summary>
    /// Получает страницу по коду
    /// </summary>
    /// <param name="pageCode">Код страницы</param>
    /// <returns>Найденная страница</returns>
    public static object ToPage(this PageCode pageCode) => pageCode switch
    {
        PageCode.ClassroomsManagement => new ClassroomsManagementPage(),
        PageCode.CreateSchedule => new CreateSchedulePage(),
        PageCode.EditSchedule => new EditSchedulePage(),
        PageCode.GroupsManagement => new GroupsManagementPage(),
        PageCode.RegisterAnnouncements => new RegisterAnnouncementsPage(),
        PageCode.SubjectsManagement => new SubjectsManagementPage(),
        PageCode.TeachersManagement => new TeachersManagementPage(),
        PageCode.CreateAnnouncements => new CreateAnnouncementPage(),
        PageCode.MyGroup => new MyGroupPage(),
        PageCode.Announcements => new AnnouncementsPage(),
        PageCode.Classrooms => new ClassroomsPage(),
        PageCode.Groups => new GroupsPage(),
        PageCode.Help => new HelpPage(),
        PageCode.Profile => new ProfilePage(),
        PageCode.Schedule => new SchedulePage(),
        PageCode.Settings => new SettingsPage(),
        PageCode.Subjects => new SubjectsPage(),
        PageCode.Teachers => new TeachersPage(),
        _ => new HomePage() // По умолчанию — главная страница
    };
}
