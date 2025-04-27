using MediatR;
using SmartCoins.Core.DTOs;

namespace SmartCoins.Core.CQRS.Queries.Expenses
{
    public class GetUserExpensesQuery : IRequest<IEnumerable<ExpenseDto>>
    {
        public int UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CategoryId { get; set; }
    }
}