using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class HelpViewModel : ObservableObject
{
    [RelayCommand]
    private void Download(string ext)
    {
        SaveFileDialog dialog = new()
        {
            Filter = ExcelManager.ExcelVersionToFileFilter(ext switch
            {
                "xlsx" => ExcelVersion.Excel2007,
                 _ => ExcelVersion.Excel97
            })
        };

        if (dialog.ShowDialog() is null) return;
        File.Copy($"pack://application,,,/TSchedule/Templates/Расписание2024_1_ИС-2.{ext}", dialog.FileName);
    }
}
