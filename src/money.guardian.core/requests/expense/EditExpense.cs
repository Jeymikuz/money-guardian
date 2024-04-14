using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.mappers;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record EditExpenseRequest(Guid Id, string Name, decimal? Value, Guid? ExpenseGroupId, string UserId)
    : IRequest<Result<ExpenseDto>>;

public class EditExpenseHandler : IRequestHandler<EditExpenseRequest, Result<ExpenseDto>>
{
    private readonly AppDbContext _appDbContext;

    public EditExpenseHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<ExpenseDto>> Handle(EditExpenseRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var expense = await _appDbContext.Expenses
            .Include(x => x.Group)
            .Where(x => x.Id == request.Id && x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (request.ExpenseGroupId.HasValue && expense.Group?.Id != request.ExpenseGroupId.GetValueOrDefault())
        {
            var expenseGroup =
                await _appDbContext.ExpenseGroups.FirstOrDefaultAsync(x => x.Id == request.ExpenseGroupId,
                    cancellationToken);

            if (expenseGroup is null)
                return null;

            expense.Group = expenseGroup;
        }

        expense.Name = request.Name ?? expense.Name;
        expense.Value = request.Value ?? expense.Value;

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return ExpenseMapper.ToExpenseDto(expense);
    }
}