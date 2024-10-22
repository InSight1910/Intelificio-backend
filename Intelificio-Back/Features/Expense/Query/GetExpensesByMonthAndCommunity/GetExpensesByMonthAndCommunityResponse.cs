using Backend.Models.Enums;

namespace Backend.Features.Expense.Query.GetExpensesByMonthAndCommunity;

public class GetExpensesByMonthAndCommunityResponse
{
    public int ExpenseId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public string ProviderRut { get; set; }
    public string Invoice { get; set; }
    public string PucharseOrder { get; set; }
}