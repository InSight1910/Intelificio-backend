using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Expenses.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Expenses.Create;

public class CreateExpenseCommandHandler(IntelificioDbContext context, IMapper mapper)
    : IRequestHandler<CreateExpenseCommand, Result>
{
    public async Task<Result> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.CommunityId))
            return Result.Failure(ExpenseErrors.CommunityNotFoundOnCreate());

        if (Enum.IsDefined(typeof(ExpenseType), request.Type))
            return Result.Failure(ExpenseErrors.ExpenseTypeNotFoundOnCreate());

        if (!await context.Expense.AnyAsync(x =>
                string.Equals(x.Invoice, request.Invoice, StringComparison.OrdinalIgnoreCase)))
            return Result.Failure(ExpenseErrors.InvoiceAlreadyExistOnCreate());
        var expense = mapper.Map<Expense>(request);
        await context.Expense.AddAsync(expense, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}