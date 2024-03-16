using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.requests.common;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record GetExpensesRequest(string Username) : IRequest<IEnumerable<ExpenseWithGroupDto>>;

public sealed class GetExpensesHandlers : IRequestHandler<GetExpensesRequest, IEnumerable<ExpenseWithGroupDto>>
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<User> _userManager;

    public GetExpensesHandlers(AppDbContext appDbContext, UserManager<User> userManager)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IEnumerable<ExpenseWithGroupDto>> Handle(GetExpensesRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null)
            return null;

        return await _appDbContext.Expenses.Include(x => x.Group)
            .Where(x => x.User.Id == user.Id)
            .Select(x => ConvertToDto(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
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