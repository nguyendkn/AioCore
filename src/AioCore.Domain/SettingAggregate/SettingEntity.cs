using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using AioCore.Shared.SeedWorks;

namespace AioCore.Domain.SettingAggregate;

public class SettingEntity : Entity
{
    public Guid TenantId { get; set; }
    
    public string Name { get; set; } = default!;

    public DataSource DataSource { get; set; }

    public string? SourcePath { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(TenantId))]
    public SettingTenant Tenant { get; set; } = default!;

    public ICollection<SettingEntityCode> EntityCodes { get; set; } = default!;

    public void Update(string? name, DataSource dataSource, string? sourcePath)
    {
        Name = string.IsNullOrEmpty(name) ? Name : name;
        DataSource = dataSource;
        SourcePath = string.IsNullOrEmpty(sourcePath) ? SourcePath : sourcePath;
        ModifiedAt = DateTime.Now;
    }
}

public enum DataSource
{
    [Description("Không xác định")]
    Undefined,
    
    [Description("Biểu mẫu nhập liệu")]
    Form,
    
    [Description("Notion")]
    Notion,
    
    [Description("GoogleSheet")]
    GoogleSheet
}