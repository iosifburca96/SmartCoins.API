using Microsoft.AspNetCore.Mvc;
using SmartCoins.Core.Exceptions;
using SmartCoins.Core.Interfaces.Services;

namespace SmartCoins.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterUser(request.Email, request.Password, request.Name);
                var token = await _authService.GenerateJwtToken(user);

                return Ok(new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Token = token
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest request)
        {
            var isValid = await _authService.ValidatePassword(request.Email, request.Password);
            if (!isValid)
                return Unauthorized(new { error = "Invalid email or password" });

            var user = await _authService.GetUserByEmail(request.Email);
            if (user == null)
                return Unauthorized(new { error = "Invalid email or password" });

            var token = await _authService.GenerateJwtToken(user);

            return Ok(new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Token = token
            });
        }
    }

    // Request and response models
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}