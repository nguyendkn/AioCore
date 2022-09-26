namespace AioCore.Shared.ValueObjects;

public static class RequestHeaders
{
    public const string Accept = nameof(Accept);
    public const string Connection = nameof(Connection);
    public const string Host = nameof(Host);
    public const string XForwardedFor = "X-Forwarded-For";
}