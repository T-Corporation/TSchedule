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
        GroupSelectionPage => PageCode.GroupSelection,
        GroupsManagementPage => PageCode.GroupsManagement,
        RegisterAnnouncementsPage => PageCode.RegisterAnnouncements,
        SpecialtiesManagementPage => PageCode.SpecialtiesManagement,
        SubjectsManagementPage => PageCode.SubjectsManagement,
        TeachersManagementPage => PageCode.TeachersManagement,
        CreateAnnouncementPage => PageCode.CreateAnnouncements,
        AnnouncementsPage => PageCode.Announcements,
        ClassroomsPage => PageCode.Classrooms,
        GroupsPage => PageCode.Groups,
        HelpPage => PageCode.Help,
        ProfilePage => PageCode.Profile,
        SchedulePage => PageCode.Schedule,
        SettingsPage => PageCode.Settings,
        SpecialtiesPage => PageCode.Specialties,
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
        PageCode.GroupSelection => new GroupSelectionPage(),
        PageCode.GroupsManagement => new GroupsManagementPage(),
        PageCode.RegisterAnnouncements => new RegisterAnnouncementsPage(),
        PageCode.SpecialtiesManagement => new SpecialtiesManagementPage(),
        PageCode.SubjectsManagement => new SubjectsManagementPage(),
        PageCode.TeachersManagement => new TeachersManagementPage(),
        PageCode.CreateAnnouncements => new CreateAnnouncementPage(),
        PageCode.Announcements => new AnnouncementsPage(),
        PageCode.Classrooms => new ClassroomsPage(),
        PageCode.Groups => new GroupsPage(),
        PageCode.Help => new HelpPage(),
        PageCode.Profile => new ProfilePage(),
        PageCode.Schedule => new SchedulePage(),
        PageCode.Settings => new SettingsPage(),
        PageCode.Specialties => new SpecialtiesPage(),
        PageCode.Subjects => new SubjectsPage(),
        PageCode.Teachers => new TeachersPage(),
        _ => new HomePage() // По умолчанию — главная страница
    };
}
