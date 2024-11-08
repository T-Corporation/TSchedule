using TSchedule.Persistence.Enums;
using TSchedule.ViewModels.Pages.ImportExportWindow;

namespace TSchedule.Views.Pages.ImportExportWindow;

public partial class SourcePage
{
    public SourcePage(WizardType wizardType)
    {
        InitializeComponent();
        var viewModel = new SourceViewModel(wizardType);
        DataContext = viewModel;
        viewModel.IsFirstRowContainsHeadersVisible = wizardType is WizardType.Import;
    }
}
