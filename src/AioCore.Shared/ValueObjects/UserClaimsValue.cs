using Newtonsoft.Json;

namespace AioCore.Shared.ValueObjects;

public class UserClaimsValue
{
    public Guid Id { get; set; }

    public string? FullName { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public List<string>? Roles { get; set; }

    public string Host { get; set; } = default!;

    public Guid TenantId { get; set; }

    public bool IsAdmin => Roles?.Contains("Admin") ?? false;
}