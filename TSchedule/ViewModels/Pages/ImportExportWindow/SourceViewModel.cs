using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.ImportExportWindow;

public partial class SourceViewModel : ObservableObject
{
    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private ExcelVersion _selectedVersion = ExcelVersion.Excel97;

    [ObservableProperty]
    private ExcelVersion[] _versions = ExcelVersion.GetAll();

    [ObservableProperty]
    private bool _firstRowContainsHeaders = true;
}
