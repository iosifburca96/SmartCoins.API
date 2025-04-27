using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartCoins.Core.Entities;
using SmartCoins.Core.Exceptions;
using SmartCoins.Core.Interfaces.Repositories;
using SmartCoins.Core.Interfaces.Services;
using SmartCoins.Infrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartCoins.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ValidatePassword(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;

            // Using BCrypt for password verification
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<User> RegisterUser(string email, string password, string name)
        {
            // Check if user exists
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                throw new BadRequestException("User with this email already exists");

            // Create new user
            var user = new User
            {
                Email = email,
                Name = name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }
    }
}