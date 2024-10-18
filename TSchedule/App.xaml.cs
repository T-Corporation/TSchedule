using System.Windows;
using TSchedule.Views;

namespace TSchedule;

public partial class App
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        WindowManager.CreateWindow<MainWindow>();
    }
}
