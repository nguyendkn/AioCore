namespace AioCore.Shared.Extensions;

public static class ConvertExtensions
{
    public static long IpToLong(this string ip)
    {
        var ipList = ip.Split('.');
        var ipNumber = Convert.ToInt64(ipList[0]) * 16777216 + Convert.ToInt64(ipList[1]) * 65536 +
                       Convert.ToInt64(ipList[2]) * 256 + Convert.ToInt64(ipList[3]);
        return ipNumber;
    }
}