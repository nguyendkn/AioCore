using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AioCore.Domain;

public class SystemAttribute
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; } = default!;

    public Guid EntityId { get; set; }

    [ForeignKey(nameof(EntityId))]
    public SystemEntity Entity { get; set; } = default!;
}