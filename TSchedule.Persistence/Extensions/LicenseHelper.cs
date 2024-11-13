using System.Text.RegularExpressions;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Extensions;

public static partial class LicenseHelper
{
    public const string EmptyKey = "XXXX-XXXX-XXXX-XXXX";

    public static bool IsLicenseValid(License license)
    {
        if (!IsKeyValid(license.Key)) return false;
        if (!IsDateValid(license.CreatedAt, license.ExpiresAt)) return false;

        var regex = LicenseKeyRegex();
        return regex.IsMatch(license.Key);
    }

    private static bool IsKeyValid(string? key) =>
        !string.IsNullOrEmpty(key) && key != EmptyKey;

    private static bool IsDateValid(DateTime createdAt, DateTime expiresAt) =>
        expiresAt > DateTime.UtcNow && createdAt <= DateTime.UtcNow;

    [GeneratedRegex(@"^\d{4}-\d{4}-\d{4}-\d{4}$")]
    private static partial Regex LicenseKeyRegex();
}
