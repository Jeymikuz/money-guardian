using MediatR;
using Microsoft.AspNetCore.Identity;
using money.guardian.core.common;
using money.guardian.core.common.errors;
using money.guardian.core.mappers;
using money.guardian.core.requests.common;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expenseGroup;

public record AddExpenseGroupRequest(string Name, string Icon, string UserId) : IRequest<Result<ExpenseGroupDto>>;

public class AddExpenseGroupHandler : IRequestHandler<AddExpenseGroupRequest, Result<ExpenseGroupDto>>
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<User> _userManager;

    public AddExpenseGroupHandler(AppDbContext appDbContext, UserManager<User> userManager)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<ExpenseGroupDto>> Handle(AddExpenseGroupRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            return new NotFoundError("User not found");

        var newGroup = CreateExpenseGroup(request, user.Id);

        await _appDbContext.ExpenseGroups.AddAsync(newGroup, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return ExpenseGroupMapper.ToExpenseGroupDto(newGroup);
    }

    private static ExpenseGroup CreateExpenseGroup(AddExpenseGroupRequest request, string userId)
        => new()
        {
            Name = request.Name,
            Icon = request.Icon,
            UserId = userId,
        };
}