using SmartCoins.Core.Entities;

namespace SmartCoins.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(User user);
        Task<bool> ValidatePassword(string email, string password);
        Task<User> RegisterUser(string email, string password, string name);
        Task<User?> GetUserByEmail(string email);
    }
}