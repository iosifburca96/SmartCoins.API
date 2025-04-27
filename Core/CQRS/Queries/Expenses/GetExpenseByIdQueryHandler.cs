using AutoMapper;
using MediatR;
using SmartCoins.Core.DTOs;
using SmartCoins.Core.Entities;
using SmartCoins.Core.Exceptions;
using SmartCoins.Core.Interfaces.Repositories;

namespace SmartCoins.Core.CQRS.Queries.Expenses
{
    public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto>
    {
        private readonly IRepository<Expense> _expenseRepository;
        private readonly IMapper _mapper;

        public GetExpenseByIdQueryHandler(
            IRepository<Expense> expenseRepository,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<ExpenseDto> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            // Get expense
            var expense = await _expenseRepository.GetByIdAsync(request.Id);

            // Validate expense exists and belongs to user
            if (expense == null)
                throw new NotFoundException($"Expense with ID {request.Id} not found");

            if (expense.UserId != request.UserId)
                throw new BadRequestException("You don't have permission to view this expense");

            // Map and return DTO
            return _mapper.Map<ExpenseDto>(expense);
        }
    }
}