using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingCode : Entity
{
    public Guid? TenantId { get; set; }
    
    public string Name { get; set; } = default!;

    public Guid? ParentId { get; set; }

    public string Code { get; set; } = "// Code Empty";
    
    [ForeignKey(nameof(ParentId))] public SettingCode? Parent { get; set; }

    public List<SettingCode> Child { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(TenantId))]
    public SettingTenant Tenant { get; set; } = default!;

    public void Update(string name, string code)
    {
        Name = string.IsNullOrEmpty(name) ? Name : name;
        Code = string.IsNullOrEmpty(code) ? Code : code;
    }
}