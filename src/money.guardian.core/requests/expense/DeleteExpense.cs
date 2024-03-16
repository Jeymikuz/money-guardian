using MediatR;
using Microsoft.EntityFrameworkCore;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public record DeleteExpenseRequest(Guid Id, string UserId) : IRequest<bool>;

public class DeleteExpenseHandler : IRequestHandler<DeleteExpenseRequest, bool>
{
    private readonly AppDbContext _appDbContext;

    public DeleteExpenseHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<bool> Handle(DeleteExpenseRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var expense = await _appDbContext.Expenses
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId, cancellationToken);

        if (expense.UserId != request.UserId)
            return false;

        _appDbContext.Expenses.Remove(expense);
        var affectedRows = await _appDbContext.SaveChangesAsync(cancellationToken);

        return affectedRows > 0;
    }
}