using Backend.Models.Base;

namespace Backend.Models
{
    public class ExpenseType : BaseEntity
    {
        public required string Description { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
