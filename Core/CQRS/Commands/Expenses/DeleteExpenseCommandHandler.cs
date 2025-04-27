using MediatR;
using SmartCoins.Core.Entities;
using SmartCoins.Core.Exceptions;
using SmartCoins.Core.Interfaces.Repositories;

namespace SmartCoins.Core.CQRS.Commands.Expenses
{
    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, bool>
    {
        private readonly IRepository<Expense> _expenseRepository;

        public DeleteExpenseCommandHandler(IRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            // Get expense
            var expense = await _expenseRepository.GetByIdAsync(request.Id);

            // Validate expense exists and belongs to user
            if (expense == null)
                throw new NotFoundException($"Expense with ID {request.Id} not found");

            if (expense.UserId != request.UserId)
                throw new BadRequestException("You don't have permission to delete this expense");

            // Delete expense
            await _expenseRepository.DeleteAsync(expense);

            return true;
        }
    }
}