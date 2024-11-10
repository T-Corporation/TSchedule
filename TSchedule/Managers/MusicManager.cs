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

    public ObservableCollection<Soundtrack> LoadSoundtracks()
    {
        ObservableCollection<Soundtrack> soundtracks = [.. LoadCoreMusicFolders()];

        foreach (var userSoundTrack in LoadUserSoundtracks())
            soundtracks.Add(userSoundTrack);

        return soundtracks;
    }

    private ICollection<Soundtrack> LoadCoreMusicFolders()
        => GetEmbeddedTracks(CoreMusicNamespace)
            .Select(file =>
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file);
                return new Soundtrack(
                    Path.GetFileNameWithoutExtension(file).Replace($"{CoreMusicNamespace}.", string.Empty),
                    "Встроенные саундтреки",
                    stream!); // Используем поток
            })
            .ToList();

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private IEnumerable<string> GetEmbeddedTracks(string coreNamespace)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(coreNamespace) && name.EndsWith(".wav"));
    }

    private ICollection<Soundtrack> LoadUserSoundtracks()
    {
        if (!Directory.Exists(UserMusicFolderPath))
            Directory.CreateDirectory(UserMusicFolderPath);

        ObservableCollection<Soundtrack> userSoundtracks = [];

        var catalogs = Directory.GetDirectories(UserMusicFolderPath)
            .Where(dir => IsValidFolderName(Path.GetFileName(dir)))
            .ToList();

        foreach (var track in catalogs.Select(
                     catalog => Directory.GetFiles(catalog, "*.wav")
                     .Select(file => new Soundtrack(
                         Path.GetFileNameWithoutExtension(file),
                         catalog.Split(@"\", 4)[3],
                         file)) // Используем путь к файлу
                     .ToList()).SelectMany(tracks => tracks))
            userSoundtracks.Add(track);

        return userSoundtracks;
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

    public void PlayTrack(Soundtrack? track, bool loop)
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
