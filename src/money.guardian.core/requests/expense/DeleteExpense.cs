using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.core.common;
using money.guardian.core.common.errors;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record DeleteExpenseRequest(Guid Id, string UserId) : IRequest<Result<Unit>>;

public class DeleteExpenseHandler : IRequestHandler<DeleteExpenseRequest, Result<Unit>>
{
    private readonly AppDbContext _appDbContext;

    public DeleteExpenseHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<Unit>> Handle(DeleteExpenseRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var expense = await _appDbContext.Expenses
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (expense is null)
            return new NotFoundError($"Entity with id {request.Id} not found");

        if (expense.UserId != request.UserId)
            return new UnAuthorizedError();

        _appDbContext.Expenses.Remove(expense);
        var affectedRows = await _appDbContext.SaveChangesAsync(cancellationToken);

        return affectedRows > 0 ? Unit.Value : new DatabaseError("Problem while deleting entity in database");
    }
}