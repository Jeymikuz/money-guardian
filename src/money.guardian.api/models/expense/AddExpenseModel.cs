namespace money.guardian.api.models.expense;

public record AddExpenseModel(string Name, decimal Value, string ExpenseGroupId);