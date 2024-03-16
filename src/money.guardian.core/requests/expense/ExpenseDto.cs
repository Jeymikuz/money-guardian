using money.guardian.core.requests.common;

namespace money.guardian.core.requests.expense;

public record ExpenseDto(string Id, string Name, decimal Value, ExpenseGroupDto ExpenseGroup, DateTimeOffset CreatedAt);