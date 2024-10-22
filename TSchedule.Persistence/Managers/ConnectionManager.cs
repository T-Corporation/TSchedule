using System.Text;
using TSchedule.Persistence.Interfaces.Managers;

namespace TSchedule.Persistence.Managers;

public class ConnectionManager : IConnectionManager
{
    /// <summary>
    /// "Ленивое" создание менеджера
    /// </summary>
    private static readonly Lazy<ConnectionManager> _instance = new(() => new ConnectionManager());

    /// <summary>
    /// Ссылка на менеджер по умолчанию
    /// </summary>
    public static ConnectionManager Default => _instance.Value;

    /// <summary>
    /// Имя пользователя ПК
    /// </summary>
    private static string AccountName => Environment.UserName;

    /// <summary>
    /// Получает строку подключения по имени пользователя
    /// </summary>
    /// <returns>Строку подключения</returns>
    public string? GetConnectionString()
    {
        ConnectionStringBuilder connectionStringBuilder = new(AuthenticationType.SqlServerAuthentication);

        switch (AccountName)
        {
            case "www":
                connectionStringBuilder.SetServer("LAPTOP-AQTL78HJ")
                    .SetDatabase("TSchedule")
                    .SetUserId("roman")
                    .SetPassword("fnaf")
                    .SetTrustServerCertificate(true);
                break;

            case "sokol":
                connectionStringBuilder.SetServer("(localdb)")
                    .SetDatabase("TSchedule")
                    .SetUserId("vadim")
                    .SetPassword("vadim")
                    .SetTrustServerCertificate(true);
                break;

            case "User":
                connectionStringBuilder.SetServer("localhost")
                    .SetDatabase("TSchedule")
                    .SetUserId("roman")
                    .SetPassword("fnaf")
                    .SetTrustServerCertificate(true);
                break;

            default:
                throw new UnauthorizedAccessException("Недопустимое имя пользователя");
        }

        return connectionStringBuilder.ToString();
    }

    /// <summary>
    /// Тип аутентификации БД
    /// </summary>
    public enum AuthenticationType
    {
        WindowsAuthentication,
        SqlServerAuthentication
    }

    /// <summary>
    /// Строитель строки подключения
    /// </summary>
    /// <param name="authenticationType">Тип аутентификации</param>
    public class ConnectionStringBuilder
        (AuthenticationType authenticationType = AuthenticationType.WindowsAuthentication)
    {
        public string Server { get; private set; } = null!;

        public string Database { get; private set; } = null!;

        public string? UserId { get; private set; }

        public string? Password { get; private set; }

        public bool TrustedConnection { get; private set; }

        public bool TrustServerCertificate { get; private set; }

        public AuthenticationType AuthenticationType { get; private set; } = authenticationType;

        public ConnectionStringBuilder SetServer(string server)
        {
            Server = server;
            return this;
        }

        public ConnectionStringBuilder SetDatabase(string database)
        {
            Database = database;
            return this;
        }

        public ConnectionStringBuilder SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }

        public ConnectionStringBuilder SetPassword(string password)
        {
            Password = password;
            return this;
        }

        public ConnectionStringBuilder SetTrustedConnection(bool trustedConnection)
        {
            TrustedConnection = trustedConnection;
            return this;
        }

        public ConnectionStringBuilder SetTrustServerCertificate(bool trustServerCertificate)
        {
            TrustServerCertificate = trustServerCertificate;
            return this;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Database) || string.IsNullOrEmpty(Server))
                throw new InvalidOperationException($"Не могу подключиться к БД без указания сервера и её имени");

            StringBuilder stringBuilder = new($"Server={Server};Database={Database};");

            switch (AuthenticationType)
            {
                case AuthenticationType.SqlServerAuthentication:
                    stringBuilder.Append($"User Id={UserId};Password={Password};");
                    break;

                case AuthenticationType.WindowsAuthentication:
                    stringBuilder.Append($"Trusted_Connection={TrustedConnection};");
                    break;
            }

            stringBuilder.Append($"TrustServerCertificate={TrustServerCertificate}");

            return stringBuilder.ToString();
        }
    }
}
