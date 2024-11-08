using System.Reflection;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Extensions;

public static class ExcelVersionExtensions
{
    public static string GetExtension(this ExcelVersion version)
    {
        var field = version.GetType().GetField(version.ToString());
        if (field?.GetCustomAttribute(typeof(ExtensionAttribute)) is ExtensionAttribute attribute)
            return attribute.Extension;
        return string.Empty;
    }
}
