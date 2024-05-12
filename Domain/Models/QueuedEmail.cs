using System.ComponentModel.DataAnnotations;
using Domain.Constants;

namespace Domain.Models;

public class QueuedEmail : Entity<long>, IAggregateRoot
{
    [Required]
    public string Email { get; set; }
    public EmailType EmailType { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? SentOn { get; set; }
}