using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.ImportExportWindow;

public class ResultViewModel(WizardResult result)
{
    public Uri ImageSource => result.Type switch
    {
        WizardResultType.Success => new Uri("pack://application:,,,/TSchedule;component/Images/verified_375.png"),
        _ => new Uri("pack://application:,,,/TSchedule;component/Images/conflict_375.png")
    };

    public string Message => result.Message;
}
