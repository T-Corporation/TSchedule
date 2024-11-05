namespace TSchedule.Extensions;

public static class Arrays
{
    /// <summary>
    /// Пытается получить значение элемента по индексу <paramref name="index"/> из массива <paramref name="array"/>
    /// </summary>
    /// <typeparam name="T">Тип элемента</typeparam>
    /// <param name="array">Массив</param>
    /// <param name="index">Индекс элемента</param>
    /// <returns><typeparamref name="T"/> элемент или его значение по умолчанию</returns>
    public static T? TryGetValue<T>(this T[] array, int index)
        => index >= 0 && index < array.Length ? array[index] : default;

    /// <summary>
    /// Пытается получить значение элемента по индексу <paramref name="index"/> из коллекции <paramref name="collection"/>
    /// </summary>
    /// <typeparam name="T">Тип элемента</typeparam>
    /// <param name="collection">Коллекция</param>
    /// <param name="index">Индекс элемента</param>
    /// <returns><typeparamref name="T"/> элемент или его значение по умолчанию</returns>
    public static T? TryGetValue<T>(this ICollection<T> collection, int index)
        => index >= 0 && index < collection.Count ? collection.ElementAt(index) : default;
}
