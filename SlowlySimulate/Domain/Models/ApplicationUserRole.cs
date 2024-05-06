﻿using Microsoft.AspNetCore.Identity;

namespace SlowlySimulate.Domain.Models;


public partial class ApplicationUserRole : IdentityUserRole<Guid>
{
    public virtual ApplicationUser User { get; set; }
    public virtual ApplicationRole Role { get; set; }
    public override Guid UserId { get => base.UserId; set => base.UserId = value; }
    public override Guid RoleId { get => base.RoleId; set => base.RoleId = value; }
}
