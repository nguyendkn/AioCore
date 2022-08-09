namespace AioCore.Shared.Extensions;

public static class StringExtension
{
    public static Guid ToGuid(this string str)
    {
        return Guid.TryParse(str, out var guid) ? guid : Guid.Empty;
    }
}