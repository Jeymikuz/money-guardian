namespace money.guardian.api.models.expense;

public record EditExpenseModel(string Name, decimal? Value, Guid? ExpenseGroupId);