using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.mappers;
using money.guardian.core.requests.common;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expenseGroup;

public record EditExpenseGroupRequest(Guid Id, string Name, string Icon, string UserId)
    : IRequest<Result<ExpenseGroupDto>>;

public class EditExpenseGroupHandler : IRequestHandler<EditExpenseGroupRequest, Result<ExpenseGroupDto>>
{
    private readonly AppDbContext _appDbContext;

    public EditExpenseGroupHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<ExpenseGroupDto>> Handle(EditExpenseGroupRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var expenseGroup = await _appDbContext.ExpenseGroups
            .Where(x => x.Id == request.Id && x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        expenseGroup.Name = request.Name ?? expenseGroup.Name;
        expenseGroup.Icon = request.Icon ?? expenseGroup.Icon;

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return ExpenseGroupMapper.ToExpenseGroupDto(expenseGroup);
    }
}