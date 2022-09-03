﻿using System.Reflection;

namespace AioCore.Shared.ValueObjects;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = default!;

    public TenantConfigs TenantConfigs { get; set; } = default!;

    public string? StorageServer { get; set; }

    public MongoConfigs MongoConfigs { get; set; } = default!;
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = default!;
}

public class MongoConfigs
{
    public string ConnectionString { get; set; } = default!;

    public string Database { get; set; } = default!;
}

public class TenantConfigs
{
    public string SavedFolder { get; set; } = default!;

    public string? AssemblySavedFolder => Path.Combine(
        Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ??
        throw new InvalidOperationException(), SavedFolder);
}