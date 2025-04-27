using MediatR;
using SmartCoins.Core.Entities;
using SmartCoins.Core.Exceptions;
using SmartCoins.Core.Interfaces.Repositories;

namespace SmartCoins.Core.CQRS.Commands.Expenses
{
    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, bool>
    {
        private readonly IRepository<Expense> _expenseRepository;

        public UpdateExpenseCommandHandler(IRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<bool> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            // Get existing expense
            var expense = await _expenseRepository.GetByIdAsync(request.Id);

            // Validate expense exists and belongs to user
            if (expense == null)
                throw new NotFoundException($"Expense with ID {request.Id} not found");

            if (expense.UserId != request.UserId)
                throw new BadRequestException("You don't have permission to update this expense");

            // Update expense properties
            expense.Amount = request.Amount;
            expense.Description = request.Description;
            expense.Date = request.Date;
            expense.CategoryId = request.CategoryId;
            expense.IsRecurring = request.IsRecurring;
            expense.UpdatedAt = DateTime.UtcNow;

            // Save changes
            await _expenseRepository.UpdateAsync(expense);

            // Handle tags (would need custom repository method)

            return true;
        }
    }
}