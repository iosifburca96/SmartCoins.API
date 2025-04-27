using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCoins.Core.CQRS.Commands.Expenses
{
    public class CreateExpenseCommand : IRequest<int>
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
        public bool IsRecurring { get; set; }
        public int UserId { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
