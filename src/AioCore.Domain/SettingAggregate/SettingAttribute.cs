using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Domain.SettingAggregate;

public class SettingAttribute : Entity
{
    public int Order { get; set; }

    public string Name { get; set; } = default!;

    public AttributeType AttributeType { get; set; }

    public Guid EntityId { get; set; }

    [ForeignKey(nameof(EntityId))] public SettingEntity Entity { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public override void ModelCreating<TSettingAttribute>(ModelBuilder modelBuilder, string schema)
    {
        var fullName = typeof(SettingAttribute).FullName;
        if (fullName != null)
        {
            var sequenceName = fullName.CreateMd5("Sequence");
            modelBuilder.HasSequence<int>(sequenceName);
            modelBuilder.Entity<SettingAttribute>().Property(o => o.Order)
                .HasDefaultValueSql($"NEXT VALUE FOR [{schema}].{sequenceName}");
            modelBuilder.Entity<SettingAttribute>().HasIndex(x => x.Order);
            modelBuilder.Entity<SettingAttribute>().Property(p => p.Name).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<SettingAttribute>().HasIndex(i => new { i.EntityId, i.Name });
        }

        modelBuilder.Entity<SettingAttribute>().HasIndex(x => x.Order);
    }

    public void Update(string name, AttributeType type)
    {
        Name = string.IsNullOrEmpty(name) ? Name : name;
        AttributeType = type;
        ModifiedAt = DateTime.Now;
    }
}

public enum AttributeType
{
    Undefined,
    Text,
    Number,
    DateTime,
    Select
}