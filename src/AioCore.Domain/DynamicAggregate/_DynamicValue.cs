using AioCore.Shared.SeedWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AioCore.Domain.DynamicAggregate;

public abstract class DynamicValue<T> : Entity
{
    public Guid AttributeId { get; set; }

    public Guid EntityId { get; set; }

    public Guid EntityTypeId { get; set; }

    public T Value { get; set; } = default!;

    public DynamicAttribute Attribute { get; set; } = default!;

    public DynamicEntity Entity { get; set; } = default!;
}

public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        Config(builder);
    }

    public abstract void Config(EntityTypeBuilder<TEntity> builder);
}