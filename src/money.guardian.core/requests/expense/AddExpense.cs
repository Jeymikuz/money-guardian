using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.requests.common;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record AddExpenseRequest(string Name, decimal Value, string UserId, string ExpenseGroupId)
    : IRequest<ExpenseDto>;

public class AddExpenseHandler : IRequestHandler<AddExpenseRequest, ExpenseDto>
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _appDbContext;

    public AddExpenseHandler(UserManager<User> userManager, AppDbContext appDbContext)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<ExpenseDto> Handle(AddExpenseRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var expenseGroup = await ResolveExpenseGroup(request.ExpenseGroupId, cancellationToken);

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            throw new ArgumentNullException(nameof(request));

        var newExpense = new Expense
        {
            Name = request.Name,
            User = user,
            Value = request.Value,
            Group = expenseGroup
        };

        await _appDbContext.Expenses.AddAsync(newExpense, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return new ExpenseDto(newExpense.Id.ToString(), newExpense.Name, newExpense.Value,
            newExpense.Group is null
                ? null
                : new ExpenseGroupDto(newExpense.Group.Id.ToString(), newExpense.Group.Name,
                    newExpense.Group.Icon, newExpense.Group.CreatedAt), newExpense.CreatedAt);
    }

    private async Task<ExpenseGroup> ResolveExpenseGroup(string expenseGroupId, CancellationToken cancellationToken)
    {
        var parseResult = Guid.TryParse(expenseGroupId, out var expenseId);

        return parseResult
            ? await _appDbContext
                .ExpenseGroups
                .FirstOrDefaultAsync(x => x.Id == expenseId, cancellationToken)
            : null;
    }
}