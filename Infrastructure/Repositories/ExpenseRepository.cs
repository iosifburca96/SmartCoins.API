using Microsoft.EntityFrameworkCore;
using SmartCoins.Core.Entities;
using SmartCoins.Core.Interfaces.Repositories;
using SmartCoins.Infrastructure.Data;

namespace SmartCoins.Infrastructure.Repositories
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IReadOnlyList<Expense>> GetByUserIdAsync(int userId)
        {
            // Include related entities for more complete data
            return await _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.Tags)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Expense>> GetByUserIdAndDateRangeAsync(
            int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.Tags)
                .Where(e => e.UserId == userId
                         && e.Date >= startDate
                         && e.Date <= endDate)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Expense>> GetByUserIdAndCategoryIdAsync(
            int userId, int categoryId)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.Tags)
                .Where(e => e.UserId == userId && e.CategoryId == categoryId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }
    }
}