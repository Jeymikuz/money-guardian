using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.requests.common;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record EditExpenseRequest(Guid Id, string Name, decimal Value, Guid ExpenseGroupId, string UserId)
    : IRequest<ExpenseDto>;

public class EditExpenseHandler : IRequestHandler<EditExpenseRequest, ExpenseDto>
{
    private readonly AppDbContext _appDbContext;

    public EditExpenseHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<ExpenseDto> Handle(EditExpenseRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var expense = await _appDbContext.Expenses
            .Include(x => x.Group)
            .Where(x => x.Id == request.Id && x.User.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ExpenseGroupId != Guid.Empty && expense.Group?.Id != request.ExpenseGroupId)
        {
            var expenseGroup =
                await _appDbContext.ExpenseGroups.FirstOrDefaultAsync(x => x.Id == request.ExpenseGroupId,
                    cancellationToken);

            if (expenseGroup is null)
                return null;

            expense.Group = expenseGroup;
        }

        expense.Name = request.Name;
        expense.Value = request.Value;

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return new ExpenseDto(expense.Id.ToString(), expense.Name, expense.Value, expense.Group is null
            ? null
            : new ExpenseGroupDto(expense.Group.Id.ToString(), expense.Group.Name,
                expense.Group.Icon, expense.Group.CreatedAt), expense.CreatedAt);
    }
}