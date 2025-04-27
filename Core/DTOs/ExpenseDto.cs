namespace SmartCoins.Core.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ReceiptUrl { get; set; }
        public bool IsRecurring { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        // public List<TagDto> Tags { get; set; } = new List<TagDto>();
    }
}
