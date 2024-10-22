using Backend.Common.Response;
using Backend.Models.Enums;
using MediatR;

namespace Backend.Features.Expenses.Create;

public class CreateExpenseCommand : IRequest<Result>
{
    public required string Name { get; set; }
    public required int Amount { get; set; }
    public required DateTime Date { get; set; }
    public required ExpenseType Type { get; set; }
    public required string ProviderRut { get; set; }
    public required string Invoice { get; set; }
    public string? PucharseOrder { get; set; }
    public required int CommunityId { get; set; }
}