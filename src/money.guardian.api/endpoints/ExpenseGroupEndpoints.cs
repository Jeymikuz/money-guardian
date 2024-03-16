using MediatR;
using money.guardian.api.models.expenseGroup;
using money.guardian.api.utilities;
using money.guardian.core.requests.common;
using money.guardian.core.requests.expense;
using money.guardian.core.requests.expenseGroup;

namespace money.guardian.api.endpoints;

public static class ExpenseGroupEndpoints
{
    private const string Prefix = "expense-group";

    public static IEndpointRouteBuilder MapExpenseGroup(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(Prefix).WithOpenApi().RequireAuthorization();

        group.MapPost(string.Empty, Create).Produces<ExpenseGroupDto>();
        return builder;
    }

    private static async Task<IResult> Create(AddExpenseGroupModel model, IMediator mediator,
        IUserAccessor userAccessor,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(model);

        var userId = userAccessor.GetId();

        var result = await mediator.Send(new AddExpenseGroupRequest(model.Name, model.Icon, userId), cancellationToken);

        return result is null ? TypedResults.Problem() : TypedResults.Ok(result);
    }
}