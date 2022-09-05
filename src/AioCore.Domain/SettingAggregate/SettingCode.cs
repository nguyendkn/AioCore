using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Domain.SettingAggregate;

public class SettingCode : Entity
{
    public Guid? TenantId { get; set; }

    public string Name { get; set; } = default!;

    public Guid? ParentId { get; set; }

    public string PathType { get; set; } = default!;

    public SaveType SaveType { get; set; } = SaveType.Undefined;

    public string Code { get; set; } = "// Code Empty";

    public bool Singled { get; set; } = false!;

    [ForeignKey(nameof(ParentId))] public SettingCode? Parent { get; set; }

    public List<SettingCode> Child { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(TenantId))] public SettingTenant Tenant { get; set; } = default!;

    public ICollection<SettingEntityCode> EntityCodes { get; set; } = default!;

    public void Update(string name, string code)
    {
        Name = string.IsNullOrEmpty(name) ? Name : name;
        Code = string.IsNullOrEmpty(code) ? Code : code;
    }

    public override void ModelCreating<T>(ModelBuilder modelBuilder, string schema)
    {
        base.ModelCreating<T>(modelBuilder, schema);
        modelBuilder.Entity<SettingCode>()
            .HasMany(u => u.EntityCodes)
            .WithOne(u => u.Code).IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public enum SaveType
{
    Undefined = 0,
    Inline = 1,
    Url = 2,
    File = 3
}