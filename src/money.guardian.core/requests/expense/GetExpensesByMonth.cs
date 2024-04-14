using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.mappers;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record GetExpensesByMonthRequest(int Month, string UserId) : IRequest<IEnumerable<ExpenseDto>>;

public class GetExpensesByMonthHandler : IRequestHandler<GetExpensesByMonthRequest, IEnumerable<ExpenseDto>>
{
    private readonly AppDbContext _appDbContext;

    public GetExpensesByMonthHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<IEnumerable<ExpenseDto>> Handle(GetExpensesByMonthRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        return await _appDbContext.Expenses
            .Include(x=>x.Group)
            .Where(x => x.UserId == request.UserId && x.CreatedAt.Month == request.Month)
            .Select(x => ExpenseMapper.ToExpenseDto(x)).ToListAsync(cancellationToken);
    }
}