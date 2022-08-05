using Microsoft.EntityFrameworkCore;

namespace AioCore.Domain;

public class AioContext : DbContext
{
    public AioContext(DbContextOptions<AioContext> options) : base(options)
    {
    }

    public DbSet<SystemAttribute> Attributes { get; set; } = default!;

    public DbSet<SystemEntity> Entities { get; set; } = default!;

    public DbSet<SystemValue> Values { get; set; } = default!;
}