using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;


public class User : IdentityUser<Guid>
{

    [MaxLength(64)]
    public string? FirstName { get; set; }

    [MaxLength(64)]
    public string? LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
