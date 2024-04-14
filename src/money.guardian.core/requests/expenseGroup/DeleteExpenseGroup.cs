using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.common.errors;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expenseGroup;

public record DeleteExpenseGroupRequest(Guid Id, string UserId) : IRequest<Result<Unit>>;

public class DeleteExpenseGroupHandler : IRequestHandler<DeleteExpenseGroupRequest, Result<Unit>>
{
    private readonly AppDbContext _appDbContext;

    public DeleteExpenseGroupHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<Unit>> Handle(DeleteExpenseGroupRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var expenseGroup =
            await _appDbContext.ExpenseGroups.Where(x => x.Id == request.Id && x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

        if (expenseGroup is null)
            return new NotFoundError($"Entity with id {request.Id} not found");

        _appDbContext.ExpenseGroups.Remove(expenseGroup);
        var affectedRows = await _appDbContext.SaveChangesAsync(cancellationToken) > 0;

        return affectedRows ? Unit.Value : new DatabaseError("Problem while deleting entity in database");
    }
}