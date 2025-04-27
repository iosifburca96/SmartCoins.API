namespace SmartCoins.Core.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}