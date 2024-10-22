using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Expenses.Query.GetExpensesByMonthAndCommunity;

public class GetExpensesByMonthAndCommunityQueryHandler(IntelificioDbContext context)
    : IRequestHandler<GetExpensesByMonthAndCommunityQuery, Result>
{
    public async Task<Result> Handle(GetExpensesByMonthAndCommunityQuery request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.Community)) return Result.Failure(null);

        var result = await context.Expense.Where(x =>
                x.CommunityId == request.Community && x.Date.Month == request.Month && x.Date.Year == request.Year)
            .Select(x => new GetExpensesByMonthAndCommunityResponse
            {
                Date = x.Date,
                ExpenseType = x.Type,
                Name = x.Name,
                ProviderRut = x.ProviderRut,
                PucharseOrder = x.PucharseOrder,
                Invoice = x.Invoice,
                ExpenseId = x.ID
            })
            .ToListAsync(cancellationToken);
        return Result.WithResponse(new ResponseData
        {
            Data = result
        });
    }
}