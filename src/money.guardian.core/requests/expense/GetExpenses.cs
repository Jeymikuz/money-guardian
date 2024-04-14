using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.common.errors;
using money.guardian.core.mappers;
using money.guardian.core.requests.common;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record GetExpensesRequest(string Username) : IRequest<Result<IEnumerable<ExpenseDto>>>;

public sealed class GetExpensesHandlers : IRequestHandler<GetExpensesRequest, Result<IEnumerable<ExpenseDto>>>
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<User> _userManager;

    public GetExpensesHandlers(AppDbContext appDbContext, UserManager<User> userManager)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<IEnumerable<ExpenseDto>>> Handle(GetExpensesRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null)
            return new UnAuthorizedError();

        return await _appDbContext.Expenses.Include(x => x.Group)
            .Where(x => x.UserId == user.Id)
            .Select(x => ExpenseMapper.ToExpenseDto(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}