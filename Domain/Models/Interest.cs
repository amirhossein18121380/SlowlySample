using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Interest : Entity<Guid>, IAggregateRoot
{
    [Required, MaxLength(50)]
    public string Name { get; set; }
}
