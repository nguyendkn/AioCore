using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Shared.SeedWorks;

public class Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public virtual void ModelCreating<T>(ModelBuilder modelBuilder, string schema) where T : Entity
    {
    }

}