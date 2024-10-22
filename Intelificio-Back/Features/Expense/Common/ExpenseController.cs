using Backend.Common.Response;
using Backend.Features.Expenses.Create;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Expenses.Common;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(
            res => Created(),
            err => BadRequest(err));
    }
}