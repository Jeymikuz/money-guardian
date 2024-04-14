using money.guardian.core.requests.common;
using money.guardian.domain.entities;

namespace money.guardian.core.mappers;

public static class ExpenseGroupMapper
{
    public static ExpenseGroupDto ToExpenseGroupDto(ExpenseGroup expenseGroup) =>
        new(expenseGroup.Id.ToString(), expenseGroup.Name, expenseGroup.Icon, expenseGroup.CreatedAt);
}