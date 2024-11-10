using System.Security.Cryptography;

namespace TSchedule.Persistence.Managers;

public class ConnectionManager
{
    public static ConnectionManager Default => _instance.Value;
    
    private static readonly byte[] IV = "ThisIsA16ByteIVv"u8.ToArray();
    private static readonly byte[] Key = "ThisIsA16ByteKey"u8.ToArray();
    private static readonly Lazy<ConnectionManager> _instance = new(() => new ConnectionManager());
    private const string EncryptedConnectionString = "+UvDo3mSWN0e+NR+W68QJ5RYuR90XuPKVqbjtEAvYXyh+dYDrqQ5tSqgsvZPoDoJMxEUwJG/3yjXg0vuxJF21FzQHr9ecYVHDRbfUGabxpKBJ5mkp13O/He3ZoxE9GA6ANUGxCJFuI3/jrkcKIRmKbZU2/rzXjF+EDwraun3EEBUCGTAS1aczOWCtn36lsSp";

    #pragma warning disable CA1822
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public string GetConnectionString()
    #pragma warning restore CA1822
    {
        return DecryptString(EncryptedConnectionString);
    }

    private static string DecryptString(string encryptedText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var encryptedBytes = Convert.FromBase64String(encryptedText);

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(encryptedBytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);
        return reader.ReadToEnd();
    }

    /*public static string EncryptString(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var writer = new StreamWriter(cs))
        {
            writer.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }*/
}
