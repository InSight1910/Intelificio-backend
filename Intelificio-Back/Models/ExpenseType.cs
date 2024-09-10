using Backend.Models.Base;

namespace Backend.Models
{
    public class ExpenseType : BaseEntity
    {
        public required string Description { get; set; }

        public required IEnumerable<Expense> Expenses { get; set; }
    }
}
