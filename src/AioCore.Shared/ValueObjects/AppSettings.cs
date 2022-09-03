using System.Reflection;
using AioCore.Notion;

namespace AioCore.Shared.ValueObjects;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = default!;

    public TenantConfigs TenantConfigs { get; set; } = default!;
    
    public string? StorageServer { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = default!;
}

public class TenantConfigs
{
    public string SavedFolder { get; set; } = default!;

    public string? AssemblySavedFolder => Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? 
            throw new InvalidOperationException(), SavedFolder);
}