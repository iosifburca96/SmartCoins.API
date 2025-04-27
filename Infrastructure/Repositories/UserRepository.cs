using Microsoft.EntityFrameworkCore;
using SmartCoins.Core.Entities;
using SmartCoins.Core.Interfaces.Repositories;
using SmartCoins.Infrastructure.Data;

namespace SmartCoins.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}