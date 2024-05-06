using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Domain.Models;

public class Topic : Entity<Guid>, IAggregateRoot
{
    [Required, MaxLength(50)]
    public string Name { get; set; }
}
