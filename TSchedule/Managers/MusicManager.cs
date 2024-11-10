using System.Collections.ObjectModel;
using System.IO;
using System.Media;
using System.Reflection;
using TSchedule.Persistence.Models;

namespace TSchedule.Managers;

public class MusicManager
{
    private const string UserMusicFolderPath = @".\Music\User";
    private const string CoreMusicNamespace = "TSchedule.Music.Core";

    public SoundPlayer SoundPlayer = new();

    public static MusicManager Default { get; } = new();

    public ObservableCollection<MusicFolder> LoadMusicFolders()
    {
        ObservableCollection<MusicFolder> musicFolders = [LoadCoreMusicFolders()];

        foreach (var userMusicFolder in LoadUserMusicFolders())
            musicFolders.Add(userMusicFolder);

        return musicFolders;
    }

    private MusicFolder LoadCoreMusicFolders()
    {
        var coreTracks = GetEmbeddedTracks(CoreMusicNamespace)
            .Select(file => 
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file);
                return new Track(Path.GetFileNameWithoutExtension(file).Replace($"{CoreMusicNamespace}.", string.Empty), stream!); // Используем поток
            })
            .ToList();

        return new MusicFolder
        {
            Name = "Встроенные саундтреки",
            Tracks = [.. coreTracks]
        };
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private IEnumerable<string> GetEmbeddedTracks(string coreNamespace)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(coreNamespace) && name.EndsWith(".wav"));
    }

    private ObservableCollection<MusicFolder> LoadUserMusicFolders()
    {
        if (!Directory.Exists(UserMusicFolderPath))
            Directory.CreateDirectory(UserMusicFolderPath);

        ObservableCollection<MusicFolder> userFolders = [];

        var directories = Directory.GetDirectories(UserMusicFolderPath)
            .Where(dir => IsValidFolderName(Path.GetFileName(dir)))
            .ToList();

        foreach (var directory in directories)
        {
            var tracks = Directory.GetFiles(directory, "*.wav")
                .Select(file => new Track(Path.GetFileNameWithoutExtension(file), file)) // Используем путь к файлу
                .ToList();
                
            userFolders.Add(new MusicFolder
            {
                Name = Path.GetFileName(directory),
                Tracks = [.. tracks]
            });
        }

        return userFolders;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private bool IsValidFolderName(string folderName)
    {
        folderName = folderName.Trim();

        if (string.IsNullOrWhiteSpace(folderName))
            return false;

        if (folderName.StartsWith('_') || folderName.EndsWith('_'))
            return false;

        return folderName.All(c => char.IsLetterOrDigit(c) || c is '_' or ' ');
    }

    public void PlayTrack(Track? track, bool loop)
    {
        if (track is null) return;

        // Создайте новый экземпляр SoundPlayer для каждого трека
        var newSoundPlayer = track.IsStream 
            ? new SoundPlayer(track.AudioStream!) 
            : new SoundPlayer(track.FilePath!);
        
        if (track.IsStream)
            track.AudioStream!.Position = 0;

        if (loop)
            newSoundPlayer.PlayLooping();
        else
            newSoundPlayer.Play();
    
        // Присваиваем новый экземпляр SoundPlayer
        SoundPlayer = newSoundPlayer;
    }


    public void Stop()
    {
        try
        {
            SoundPlayer.Stop();
        }
        catch (InvalidOperationException)
        {
            // Игнорируем исключение, если SoundPlayer не играл
        }
    }
}
