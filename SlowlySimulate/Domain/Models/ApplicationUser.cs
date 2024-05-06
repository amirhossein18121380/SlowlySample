using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Domain.Models;


public class ApplicationUser : IdentityUser<Guid>
{

    [MaxLength(64)]
    public string? FirstName { get; set; }

    [MaxLength(64)]
    public string? LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    public SlowlyUser SlowlyUser { get; set; }
}
