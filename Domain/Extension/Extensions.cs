using Microsoft.AspNetCore.Identity;

namespace Domain.Extension
{
    public static class Extensions
    {
        public static string GetErrors(this IdentityResult result)
        {
            return string.Join("\n", result.Errors.Select(i => i.Description));
        }
    }
}
