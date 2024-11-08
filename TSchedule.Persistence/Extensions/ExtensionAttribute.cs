namespace TSchedule.Persistence.Extensions;

[AttributeUsage(AttributeTargets.Field)]
public class ExtensionAttribute(string extension) : Attribute
{
    public string Extension { get; } = extension;
}
