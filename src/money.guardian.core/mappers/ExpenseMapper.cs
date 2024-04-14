using money.guardian.core.requests.common;
using money.guardian.core.requests.expense;
using money.guardian.domain.entities;

namespace money.guardian.core.mappers;

public static class ExpenseMapper
{
    public static ExpenseDto ToExpenseDto(Expense expense) =>
        new(expense.Id.ToString(),
            expense.Name,
            expense.Value,
            expense.Group is null
                ? null
                : new ExpenseGroupDto(expense.Group.Id.ToString(), expense.Group.Name,
                    expense.Group.Icon, expense.Group.CreatedAt), expense.CreatedAt);
}