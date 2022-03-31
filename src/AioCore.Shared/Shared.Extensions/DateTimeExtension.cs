using System.Globalization;

namespace Shared.Extensions;

public static class DateTimeExtension
{
    public static DateTimeOffset ToDateTime(this string hexCode)
    {
        var dec = long.Parse(hexCode, NumberStyles.HexNumber);
        return dec.ToDateTime();
    }

    public static string ToHexCode(this DateTimeOffset dateTime)
    {
        var unix = dateTime.ToUnixTimeSeconds();
        return unix.ToString("X");
    }

    private static DateTimeOffset ToDateTime(this long ticks)
    {
        return DateTimeOffset.FromUnixTimeSeconds(ticks);
    }
}