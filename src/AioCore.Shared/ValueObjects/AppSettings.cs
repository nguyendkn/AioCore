namespace AioCore.Shared.ValueObjects;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = default!;

    public TenantConfigs TenantConfigs { get; set; } = default!;
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = default!;
}

public class TenantConfigs
{
    public string SavedFolder { get; set; } = default!;
}