using money.guardian.core.requests.common;
using money.guardian.domain.entities;

namespace money.guardian.core.requests.expense;

public record ExpenseWithGroupDto(
    string Id,
    string Name,
    decimal Value,
    ExpenseGroupDto ExpenseGroup,
    DateTimeOffset CreatedAt);