using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public partial class Role : IdentityRole<Guid>
{
    public Role() : base() { }
    public Role(string roleName) : base(roleName) { }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }
}