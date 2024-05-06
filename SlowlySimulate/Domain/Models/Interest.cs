using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Domain.Models;

public class Interest : Entity<Guid>, IAggregateRoot
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string Name { get; set; }
    public ICollection<ApplicationUser> Users { get; set; }
}
