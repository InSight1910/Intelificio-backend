using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models;

public class Expense : BaseEntity
{
    public string Name { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
    public ExpenseType Type { get; set; }
    public string ProviderRut { get; set; }
    public string Invoice { get; set; }
    public string? PucharseOrder { get; set; }
    public required int CommunityId { get; set; }
    public Community Community { get; set; }
}