using MediatR;
using SmartCoins.Core.Entities;
using SmartCoins.Core.Exceptions;
using SmartCoins.Core.Interfaces.Repositories;

namespace SmartCoins.Core.CQRS.Commands.Expenses
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, int>
    {
        private readonly IRepository<Expense> _expenseRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<ExpenseCategory> _categoryRepository;

        public CreateExpenseCommandHandler(
            IRepository<Expense> expenseRepository,
            IRepository<Tag> tagRepository,
            IRepository<ExpenseCategory> categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<int> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            // Validate category if provided
            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
                if (category == null || category.UserId != request.UserId)
                {
                    throw new BadRequestException("Invalid category specified");
                }
            }

            // Create new expense
            var expense = new Expense
            {
                Amount = request.Amount,
                Description = request.Description,
                Date = request.Date,
                CategoryId = request.CategoryId,
                IsRecurring = request.IsRecurring,
                UserId = request.UserId,
                ReceiptUrl = string.Empty, // Will be updated later if needed
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add expense first to get the ID
            var createdExpense = await _expenseRepository.AddAsync(expense);

            // Then add tags if any
            if (request.TagIds.Any())
            {
                // This needs custom repository logic since we're working with a many-to-many relationship
                // For now, we'd have to implement this in a specialized repository method
            }

            return createdExpense.Id;
        }
    }
}