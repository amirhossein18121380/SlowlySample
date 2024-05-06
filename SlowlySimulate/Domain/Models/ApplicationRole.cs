﻿using Microsoft.AspNetCore.Identity;

namespace SlowlySimulate.Domain.Models;

public partial class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }
    public override Guid Id { get => base.Id; set => base.Id = value; }
    public override string? Name { get => base.Name; set => base.Name = value; }
    public override string? NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }
    public override string? ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
}