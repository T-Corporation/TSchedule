namespace TSchedule.Persistence.Models;

/// <summary>
/// Саундтрек
/// </summary>
public class Track
{
    /// <summary>
    /// Название трека
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Поток для воспроизведения аудио (если используется поток)
    /// </summary>
    public Stream? AudioStream { get; }

    /// <summary>
    /// Путь к файлу (если используется путь к файлу)
    /// </summary>
    public string? FilePath { get; }

    /// <summary>
    /// Определяет, содержит ли трек поток или путь к файлу
    /// </summary>
    public bool IsStream { get; }

    /// <summary>
    /// Конструктор для создания трека с потоком
    /// </summary>
    /// <param name="name">Название саундтрека</param>
    /// <param name="audioStream">Поток воспроизведения</param>
    public Track(string name, Stream audioStream)
    {
        Name = name;
        AudioStream = audioStream;
        IsStream = true;
    }

    /// <summary>
    /// Конструктор для создания трека с путем к файлу
    /// </summary>
    /// <param name="name">Название саундтрека</param>
    /// <param name="filePath">Путь к файлу</param>
    public Track(string name, string filePath)
    {
        Name = name;
        FilePath = filePath;
        IsStream = false;
    }
}
