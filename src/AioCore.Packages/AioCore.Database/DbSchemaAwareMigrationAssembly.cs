using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace AioCore.Database;

#pragma warning disable EF1001
public class DbSchemaAwareMigrationAssembly : MigrationsAssembly
{
    private readonly DbContext _context;
    
    public DbSchemaAwareMigrationAssembly(ICurrentDbContext currentContext, 
        IDbContextOptions options, IMigrationsIdGenerator idGenerator, 
        IDiagnosticsLogger<DbLoggerCategory.Migrations> logger) : 
        base(currentContext, options, idGenerator, logger)
    {
        _context = currentContext.Context;
    }
    
    /// <inheritdoc />
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
    {
        if (activeProvider == null)
            throw new ArgumentNullException(nameof(activeProvider));

        var hasCtorWithSchema = migrationClass.GetConstructor(new[] { typeof(IDbContextSchema) }) != null;
        
        if (!hasCtorWithSchema || _context is not IDbContextSchema schema)
            return base.CreateMigration(migrationClass, activeProvider);
        var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), schema)!;
        instance.ActiveProvider = activeProvider;
        return instance;

    }
}