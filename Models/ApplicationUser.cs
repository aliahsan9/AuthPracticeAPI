using Microsoft.AspNetCore.Identity;

namespace AuthPracticeAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
