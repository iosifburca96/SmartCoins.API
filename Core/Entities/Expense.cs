namespace SmartCoins.Core.Entities
{
    public class Expense : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string ReceiptUrl { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public ExpenseCategory? Category { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}