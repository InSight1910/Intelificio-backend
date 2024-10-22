using Backend.Common.Response;

namespace Backend.Features.Expenses.Common;

public class ExpenseErrors
{
    public static Error CommunityNotFoundOnCreate()
    {
        return new Error
        {
            Code = "Expense.Create.CommunityNotFoundOnCreate",
            Message = "La comunidad no fue encontrada."
        };
    }

    public static Error ExpenseTypeNotFoundOnCreate()
    {
        return new Error
        {
            Code = "Expense.Create.ExpenseTypeNotFoundOnCreate",
            Message = "El tipo de gasto no es valido."
        };
    }

    public static Error InvoiceAlreadyExistOnCreate()
    {
        return new Error
        {
            Code = "Expense.Create.InvoiceAlreadyExistOnCreate",
            Message = "Ya existe una factura registrada con esa identificacion."
        };
    }
}