using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AioCore.Domain.IdentityAggregate;

public class Role : IdentityRole<Guid>
{
    public Guid? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public Role? Parent { get; set; }
}