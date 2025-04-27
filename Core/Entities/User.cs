namespace SmartCoins.Core.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<ExpenseCategory> Categories { get; set; } = new List<ExpenseCategory>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<SavingsGoal> SavingsGoals { get; set; } = new List<SavingsGoal>();
    }
}