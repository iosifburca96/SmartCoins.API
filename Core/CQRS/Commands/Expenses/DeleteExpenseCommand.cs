using MediatR;

namespace SmartCoins.Core.CQRS.Commands.Expenses
{
    public class DeleteExpenseCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int UserId { get; set; } // For security validation
    }
}