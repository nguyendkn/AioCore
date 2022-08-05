using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AioCore.Domain;

public class SystemValue
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid AttributeId { get; set; }

    public Guid EntityId { get; set; }

    public Guid EntityTypeId { get; set; }

    public string? StringValue { get; set; }

    public double? NumberValue { get; set; }

    public string? ConstantValue { get; set; }

    public DateTime DateTimeValue { get; set; }

    public Guid GuidValue { get; set; }

    public virtual SystemAttribute Attribute { get; set; } = default!;

    public virtual SystemEntity Entity { get; set; } = default!;
}