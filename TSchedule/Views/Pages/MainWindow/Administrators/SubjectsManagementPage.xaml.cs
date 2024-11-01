using System.Windows;
using TSchedule.ViewModels.Pages.MainWindow.Administrators;

namespace TSchedule.Views.Pages.MainWindow.Administrators;

public partial class SubjectsManagementPage
{
    public SubjectsManagementPage() => InitializeComponent();
    
    private async void Page_Loaded(object sender, RoutedEventArgs e)
        => DataContext = await SubjectsManagementViewModel.CreateInstanceAsync(AddButton, SubjectsFlyout);
}