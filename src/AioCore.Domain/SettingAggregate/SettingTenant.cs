using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingTenant : Entity
{
    public string Name { get; set; } = default!;

    public string Domain { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string? Keyword { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public Guid? GroupId { get; set; }

    public ICollection<SettingCode> Codes { get; set; } = default!;

    public ICollection<SettingTenantDomain> Domains { get; set; } = default!;
    
    [ForeignKey(nameof(GroupId))] public SettingTenantGroup Group { get; set; } = default!;
}

public class SettingTenantDomain : Entity
{
    public string Domain { get; set; } = default!;

    public bool Active { get; set; }

    public Guid TenantId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    
    [ForeignKey(nameof(TenantId))] public SettingTenant Tenant { get; set; } = default!;

    public void Update(string domain)
    {
        Domain = string.IsNullOrEmpty(domain) ? Domain : domain;
    }
}

public class SettingTenantGroup : Entity
{
    public string Name { get; set; } = default!;

    public List<SettingTenant> Tenants { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public void Update(string name)
    {
        Name = string.IsNullOrEmpty(name) ? Name : name;
        ModifiedAt = DateTime.Now;
    }
}