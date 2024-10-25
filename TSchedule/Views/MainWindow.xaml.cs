using iNKORE.UI.WPF.Modern.Controls;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.ViewModels;
using TSchedule.Views.Pages.MainWindow;
using TSchedule.Views.Pages.MainWindow.Administrators;
using TSchedule.Views.Pages.MainWindow.Teachers;

namespace TSchedule.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(ContentFrame);
    }

    private void NavigationView_ItemInvoked(NavigationView _, NavigationViewItemInvokedEventArgs args)
    {
        WindowManager.Default.GetViewModel<MainWindow>()
            !.As<MainWindowViewModel>()
            !.NavigationFrame.Navigate((PageCode)args.InvokedItemContainer.Tag switch
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

                    PageCode.None or PageCode.Home or _ => new HomePage()
                });
    }
}
