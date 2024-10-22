using AutoMapper;
using Backend.Features.Expenses.Create;
using Backend.Models;

namespace Backend.Common.Profiles;

public class ExpenseProfile : Profile
{
    public ExpenseProfile()
    {
        CreateMap<CreateExpenseCommand, Expense>();
    }
}