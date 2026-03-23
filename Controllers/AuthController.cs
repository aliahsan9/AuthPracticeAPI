using AuthPracticeAPI.Data;
using AuthPracticeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace AuthPracticeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public AuthController(UserManager<ApplicationUser> manager, IConfiguration config)
        {
            _userManager = manager;
            _config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register (RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
               

            }   
            return Ok("User Created");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            var token = GeterateJwtToken(user);
            return Ok(new { token });
        }

    }

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
}

