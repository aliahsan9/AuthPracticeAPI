using AuthPracticeAPI.Controllers;
using AuthPracticeAPI.Models;
using System.Threading.Tasks;

namespace AuthPracticeAPI.Services
{
    public interface IAuthService
    {
        // Authentication
        Task<string> RegisterAsync(RegisterDto model);
        Task<string> LoginAsync(LoginDto model);

        // JWT
        string GenerateJwtToken(ApplicationUser user);

        // External OAuth
        Task<string> ExternalLoginAsync(string provider, string idToken);

        // Forgot Password
        Task<string> ForgotPasswordAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordDto model);
    }
}