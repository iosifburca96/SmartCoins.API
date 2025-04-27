using MediatR;
using SmartCoins.Core.DTOs;

namespace SmartCoins.Core.CQRS.Queries.Expenses
{
    public class GetExpenseByIdQuery : IRequest<ExpenseDto>
    {
        public int Id { get; set; }
        public int UserId { get; set; } // For security validation
    }
}