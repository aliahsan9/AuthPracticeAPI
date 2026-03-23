using AuthPracticeAPI.Controllers;
using AuthPracticeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthPracticeAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // You can implement RegisterAsync, LoginAsync, ExternalLoginAsync etc. here
        public async Task<string> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(e => e.Description));

            return GenerateJwtToken(user);
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return null;

            return GenerateJwtToken(user);
        }

        public Task<string> ExternalLoginAsync(string provider, string idToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> ForgotPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> ResetPasswordAsync(ResetPasswordDto model)
        {
            throw new NotImplementedException();
        }
    }
}