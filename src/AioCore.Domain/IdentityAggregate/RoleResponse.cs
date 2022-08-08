namespace AioCore.Domain.IdentityAggregate;

public class RoleResponse
{
    public Guid Id { get; set; }

    public int Index { get; set; }
    
    public string Name { get; set; } = default!;

    public string Href { get; set; } = default!;

    public string Icon { get; set; } = default!;
    
    public Guid? ParentId { get; set; }

    public List<RoleResponse> Children { get; set; } = default!;
}