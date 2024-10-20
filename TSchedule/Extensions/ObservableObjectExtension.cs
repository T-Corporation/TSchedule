using CommunityToolkit.Mvvm.ComponentModel;

namespace TSchedule.Extensions;

public static class ObservableObjectExtension
{
    /// <summary>
    /// Преобразует DataContext к указанному типу
    /// </summary>
    /// <typeparam name="T">Тип DataContext</typeparam>
    /// <param name="viewModel">DataContext окна</param>
    /// <returns>DataContext, приведённый к типу <typeparamref name="T"/></returns>
    public static T? As<T>(this ObservableObject viewModel) where T : ObservableObject => viewModel as T;
}
