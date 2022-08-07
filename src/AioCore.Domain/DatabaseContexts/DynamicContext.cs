using AioCore.Domain.DynamicAggregate;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Domain.DatabaseContexts;

public class DynamicContext : DbContext
{
    public DynamicContext(DbContextOptions<DynamicContext> options) : base(options)
    {
    }
    
    public DbSet<DynamicAttribute> Attributes { get; set; } = default!;

    public DbSet<DynamicEntity> Entities { get; set; } = default!;

    public DbSet<DynamicValue> Values { get; set; } = default!;
}