using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Domain.Models;

public class Stamp : Entity<Guid>, IAggregateRoot
{
    [Required, MaxLength(50)]
    public string Country { get; set; }
    [Required, MaxLength(255)]
    public string ImageUrl { get; set; }
}
