using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.mappers;
using money.guardian.core.requests.common;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expenseGroup;

public record GetExpenseGroupsRequest(string UserId) : IRequest<Result<IEnumerable<ExpenseGroupDto>>>;

public class GetExpenseGroupsHandler : IRequestHandler<GetExpenseGroupsRequest, Result<IEnumerable<ExpenseGroupDto>>>
{
    private readonly AppDbContext _appDbContext;

    public GetExpenseGroupsHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<IEnumerable<ExpenseGroupDto>>> Handle(GetExpenseGroupsRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        return await _appDbContext.ExpenseGroups
            .Where(x => x.UserId == request.UserId)
            .Select(group => ExpenseGroupMapper.ToExpenseGroupDto(group))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}