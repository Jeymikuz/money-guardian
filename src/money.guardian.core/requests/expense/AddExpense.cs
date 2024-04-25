using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.common.errors;
using money.guardian.core.mappers;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record AddExpenseRequest(string Name, decimal Value, string UserId, string ExpenseGroupId)
    : IRequest<Result<ExpenseDto>>;

public class AddExpenseHandler : IRequestHandler<AddExpenseRequest, Result<ExpenseDto>>
{
    private readonly AppDbContext _appDbContext;

    public AddExpenseHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<ExpenseDto>> Handle(AddExpenseRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var expenseGroup = await ResolveExpenseGroup(request.ExpenseGroupId, cancellationToken);
        if (expenseGroup.IsError)
            return expenseGroup.Error;

        var newExpense = NewExpense(request, expenseGroup);

        await _appDbContext.Expenses.AddAsync(newExpense, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return ExpenseMapper.ToExpenseDto(newExpense);
    }

    private static Expense NewExpense(AddExpenseRequest request, Result<ExpenseGroup> expenseGroup)
        => new()
        {
            Name = request.Name,
            Value = request.Value,
            Group = expenseGroup.Value,
            UserId = request.UserId
        };

    private async Task<Result<ExpenseGroup>> ResolveExpenseGroup(string expenseGroupId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(expenseGroupId))
            return (ExpenseGroup)null;

        return await GetExpenseGroup(expenseGroupId, cancellationToken);
    }

    private async Task<Result<ExpenseGroup>> GetExpenseGroup(string expenseGroupId,
        CancellationToken cancellationToken)
    {
        var parseResult = Guid.TryParse(expenseGroupId, out var expenseId);
        if (!parseResult)
            return new BaseError("Incorrect id format");

        var expenseGroup = await _appDbContext
            .ExpenseGroups
            .FirstOrDefaultAsync(x => x.Id == expenseId, cancellationToken);

        return expenseGroup is null ? new NotFoundError($"Expense group with id {expenseId} not found") : expenseGroup;
    }
}