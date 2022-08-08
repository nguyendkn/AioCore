using AioCore.Domain.IdentityAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Domain.DatabaseContexts;

public class IdentityContext : IdentityDbContext<User, Role, Guid>
{
    public const string Schema = "Identity";
    
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(Schema);
    }
}