using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Domain.Models;


public class SubTopic : Entity<Guid>, IAggregateRoot
{
    [Required, MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public Guid TopicId { get; set; }
}