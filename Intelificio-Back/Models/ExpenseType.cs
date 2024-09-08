using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class ExpenseType : BaseEntity
    {
        public required string Description { get; set; }

        public required IEnumerable<Expense> Expenses { get; set; }
    }
}
