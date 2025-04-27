using AutoMapper;
using MediatR;
using SmartCoins.Core.DTOs;
using SmartCoins.Core.Entities;
using SmartCoins.Core.Interfaces.Repositories;

namespace SmartCoins.Core.CQRS.Queries.Expenses
{
    public class GetUserExpensesQueryHandler : IRequestHandler<GetUserExpensesQuery, IEnumerable<ExpenseDto>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public GetUserExpensesQueryHandler(
            IExpenseRepository expenseRepository,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDto>> Handle(GetUserExpensesQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Expense> expenses;

            // Apply filters if provided
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                expenses = await _expenseRepository.GetByUserIdAndDateRangeAsync(
                    request.UserId,
                    request.StartDate.Value,
                    request.EndDate.Value);
            }
            else if (request.CategoryId.HasValue)
            {
                expenses = await _expenseRepository.GetByUserIdAndCategoryIdAsync(
                    request.UserId,
                    request.CategoryId.Value);
            }
            else
            {
                expenses = await _expenseRepository.GetByUserIdAsync(request.UserId);
            }

            // Map and return DTOs
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }
    }
}