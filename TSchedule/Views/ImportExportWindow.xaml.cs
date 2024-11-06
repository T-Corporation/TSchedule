using System.Windows;
using TSchedule.Persistence.Enums;
using TSchedule.ViewModels;

namespace TSchedule.Views;

public partial class ImportExportWindow
{
    private readonly WizardType _wizardType;

    public ImportExportWindow(WizardType wizardType = WizardType.Export)
    {
        InitializeComponent();
        _wizardType = wizardType;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
        => DataContext = new ImportExportViewModel(NavigationFrame, _wizardType);
}
