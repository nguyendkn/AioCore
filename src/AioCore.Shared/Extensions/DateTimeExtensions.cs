namespace AioCore.Shared.Extensions;

public static class DateTimeExtensions
{
    public static string ToHexCode(this DateTimeOffset dateTime)
    {
        var unix = dateTime.ToUnixTimeSeconds();
        return unix.ToString("X");
    }

    public static string ToHexCode(this DateTime dateTime)
    {
        return dateTime.Ticks.ToString("X2");
    }
}