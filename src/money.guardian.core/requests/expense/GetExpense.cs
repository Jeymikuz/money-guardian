using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.requests.common;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record GetExpenseRequest(Guid Id, string UserId) : IRequest<ExpenseWithGroupDto>;

public class GetExpenseHandler : IRequestHandler<GetExpenseRequest, ExpenseWithGroupDto>
{
    private readonly AppDbContext _appDbContext;

    public GetExpenseHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<ExpenseWithGroupDto> Handle(GetExpenseRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        return await _appDbContext.Expenses.Include(x => x.Group)
            .Where(x => x.Id == request.Id && x.UserId == request.UserId)
            .Select(x => ConvertToDto(x))
            .AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    private static ExpenseWithGroupDto ConvertToDto(Expense expense) =>
        new(expense.Id.ToString(),
            expense.Name,
            expense.Value,
            expense.Group == null
                ? null
                : new ExpenseGroupDto(expense.Group.Id.ToString(), expense.Group.Name, expense.Group.Icon,
                    expense.Group.CreatedAt),
            expense.CreatedAt);
}