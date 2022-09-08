using Microsoft.AspNetCore.Identity;

namespace AioCore.Domain.IdentityAggregate;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = default!;

    public bool Locked { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public void Create(string? email, string? fullName)
    {
        Email = email ?? Email;
        FullName = fullName ?? FullName;
        UserName = Email.Split("@").FirstOrDefault() ?? UserName;
        CreatedAt = DateTime.Now;
        ModifiedAt = DateTime.Now;
    }
}