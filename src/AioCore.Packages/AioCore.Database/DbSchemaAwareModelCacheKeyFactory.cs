using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AioCore.Database;

public class DbSchemaAwareModelCacheKeyFactory : IModelCacheKeyFactory
{
    /// <inheritdoc />
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public object Create(DbContext context)
    {
        return new { Type = context.GetType(), Schema = context is IDbContextSchema schema ? schema.Schema : null };
    }
}