using SmartCoins.Core.Entities;

namespace SmartCoins.Core.Interfaces.Repositories
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<IReadOnlyList<Expense>> GetByUserIdAsync(int userId);
        Task<IReadOnlyList<Expense>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<Expense>> GetByUserIdAndCategoryIdAsync(int userId, int categoryId);
    }
}
