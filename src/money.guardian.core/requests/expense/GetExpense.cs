using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.mappers;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record GetExpenseRequest(Guid Id, string UserId) : IRequest<Result<ExpenseDto>>;

public class GetExpenseHandler : IRequestHandler<GetExpenseRequest, Result<ExpenseDto>>
{
    private readonly AppDbContext _appDbContext;

    public GetExpenseHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<ExpenseDto>> Handle(GetExpenseRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        return await _appDbContext.Expenses.Include(x => x.Group)
            .Where(x => x.Id == request.Id && x.UserId == request.UserId)
            .Select(x => ExpenseMapper.ToExpenseDto(x))
            .AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }
}