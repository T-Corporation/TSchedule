using TSchedule.Persistence.Models;
using TSchedule.ViewModels.Pages.ImportExportWindow;

namespace TSchedule.Views.Pages.ImportExportWindow;

public partial class ResultPage
{
    public ResultPage(WizardResult result)
    {
        InitializeComponent();
        DataContext = new ResultViewModel(result);
    }
}
