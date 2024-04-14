using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.mappers;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record AddExpenseRequest(string Name, decimal Value, string UserId, string ExpenseGroupId)
    : IRequest<Result<ExpenseDto>>;

public class AddExpenseHandler : IRequestHandler<AddExpenseRequest, Result<ExpenseDto>>
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _appDbContext;

    public AddExpenseHandler(UserManager<User> userManager, AppDbContext appDbContext)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<ExpenseDto>> Handle(AddExpenseRequest request, CancellationToken cancellationToken)
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
            Value = request.Value,
            Group = expenseGroup,
            UserId = user.Id
        };

        await _appDbContext.Expenses.AddAsync(newExpense, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return ExpenseMapper.ToExpenseDto(newExpense);
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