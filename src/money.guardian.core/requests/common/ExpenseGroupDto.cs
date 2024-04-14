using money.guardian.domain.entities;

namespace money.guardian.core.requests.common;

public record ExpenseGroupDto(string Id, string Name, string Icon, DateTimeOffset CreatedAt);