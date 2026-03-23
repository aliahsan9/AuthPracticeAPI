using AuthPracticeAPI.Models;
using AuthPracticeAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthPracticeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(IAuthService authService, SignInManager<ApplicationUser> signInManager)
        {
            _authService = authService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var tokenOrError = await _authService.RegisterAsync(model);
            if (tokenOrError.Contains(" "))
                return BadRequest(tokenOrError);

            return Ok(new { token = tokenOrError });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var token = await _authService.LoginAsync(model);
            if (token == null)
                return Unauthorized();

            return Ok(new { token });
        }

        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
                return BadRequest();

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                false);

            if (!signInResult.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                await _signInManager.UserManager.CreateAsync(user);
                await _signInManager.UserManager.AddLoginAsync(user, info);
            }

            return Ok("External login success");
        }
    }

    // DTOs
    public class RegisterDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}