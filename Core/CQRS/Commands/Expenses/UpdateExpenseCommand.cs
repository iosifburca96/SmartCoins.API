using MediatR;

namespace SmartCoins.Core.CQRS.Commands.Expenses
{
    public class UpdateExpenseCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
        public bool IsRecurring { get; set; }
        public int UserId { get; set; } // For security validation
        public List<int> TagIds { get; set; } = new List<int>();
    }
}