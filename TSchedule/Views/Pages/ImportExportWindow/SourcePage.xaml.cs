using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Models;
using TSchedule.ViewModels.Pages.ImportExportWindow;

namespace TSchedule.Views.Pages.ImportExportWindow;

public partial class SourcePage
{
    public SourcePage(GroupModel group, byte semester, short year, WizardType wizardType)
    {
        InitializeComponent();
        DataContext = new SourceViewModel(group, semester, year, wizardType);
    }
}
