using System.ComponentModel.DataAnnotations;

namespace Domain.Dto.Role
{
    public class RoleDto
    {
        [StringLength(64, ErrorMessage = "ErrorInvalidLength", MinimumLength = 2)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "SpacesNotPermitted")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public List<string> Permissions { get; set; }

        public string FormattedPermissions
        {
            get
            {
                return string.Join(", ", Permissions.ToArray());
            }
        }
    }
}
