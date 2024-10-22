using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Expense.Query.GetExpensesByMonthAndCommunity;

public class GetExpensesByMonthAndCommunityQuery : IRequest<Result>
{
    public required int Month { get; set; }
    public required int Year { get; set; }
    public required int Community { get; set; }
}