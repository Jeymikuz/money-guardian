using MediatR;
using Microsoft.AspNetCore.Identity;
using money.guardian.core.requests.common;
using money.guardian.core.requests.expense;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expenseGroup;

public record AddExpenseGroupRequest(string Name, string Icon, string UserId) : IRequest<ExpenseGroupDto>;

public class AddExpenseGroupHandler : IRequestHandler<AddExpenseGroupRequest, ExpenseGroupDto>
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<User> _userManager;

    public AddExpenseGroupHandler(AppDbContext appDbContext, UserManager<User> userManager)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<ExpenseGroupDto> Handle(AddExpenseGroupRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            return null;

        var newGroup = CreateGroup(request, user);

        await _appDbContext.ExpenseGroups.AddAsync(newGroup, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return new ExpenseGroupDto(newGroup.Id.ToString(), newGroup.Name, newGroup.Icon, newGroup.CreatedAt);
    }

    private static ExpenseGroup CreateGroup(AddExpenseGroupRequest request, User user)
        => new()
        {
            Name = request.Name,
            Icon = request.Icon,
            User = user,
        };
}