using SlowlySimulate.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Domain.Models;

public class QueuedEmail : Entity<long>, IAggregateRoot
{
    [Required]
    public string Email { get; set; }
    public EmailType EmailType { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? SentOn { get; set; }
}