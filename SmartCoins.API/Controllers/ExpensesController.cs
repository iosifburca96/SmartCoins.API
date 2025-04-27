using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCoins.Core.CQRS.Commands.Expenses;
using SmartCoins.Core.CQRS.Queries.Expenses;
using SmartCoins.Core.DTOs;
using System.Security.Claims;

namespace SmartCoins.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpensesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? categoryId)
        {
            // Get user ID from token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim);

            // Create query
            var query = new GetUserExpensesQuery
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
                CategoryId = categoryId
            };

            // Execute query
            var expenses = await _mediator.Send(query);

            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
        {
            // Get user ID from token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim);

            // Create query
            var query = new GetExpenseByIdQuery { Id = id, UserId = userId };

            // Execute query
            var expense = await _mediator.Send(query);

            return Ok(expense);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateExpense(CreateExpenseCommand command)
        {
            // Get user ID from token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim);

            // Set user ID on command
            command.UserId = userId;

            // Execute command
            var expenseId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetExpense), new { id = expenseId }, expenseId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, UpdateExpenseCommand command)
        {
            // Get user ID from token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim);

            // Set ID and user ID on command
            command.Id = id;
            command.UserId = userId;

            // Execute command
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            // Get user ID from token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim);

            // Create command
            var command = new DeleteExpenseCommand { Id = id, UserId = userId };

            // Execute command
            await _mediator.Send(command);

            return NoContent();
        }

        /*[HttpPost("{id}/receipt")]
        public async Task<IActionResult> UploadReceipt(int id, IFormFile file)
        {
            // Get user ID from JWT token claims
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Create command
            var command = new UploadReceiptCommand
            {
                ExpenseId = id,
                UserId = userId,
                File = file
            };

            // Send command through mediator
            await _mediator.Send(command);

            return NoContent();
        }*/

        /*[HttpGet("report")]
        public async Task<ActionResult<ExpenseReportDto>> GetExpenseReport(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? categoryId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = new GetExpenseReportQuery
            {
                UserId = userId,
                StartDate = startDate ?? DateTime.UtcNow.AddMonths(-1),
                EndDate = endDate ?? DateTime.UtcNow,
                CategoryId = categoryId
            };

            var report = await _mediator.Send(query);

            return Ok(report);
        }*/
    }
}
