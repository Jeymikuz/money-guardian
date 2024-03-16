using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using money.guardian.api.models.expense;
using money.guardian.api.utilities;
using money.guardian.core.requests.expense;

namespace money.guardian.api.endpoints;

public static class ExpenseEndpoints
{
    private const string Prefix = "expense";

    public static IEndpointRouteBuilder MapExpense(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(Prefix).WithOpenApi().RequireAuthorization();

        group.MapPost(string.Empty, Create).Produces<ExpenseDto>();
        group.MapPut("{id}", Edit);
        group.MapGet(string.Empty, GetAll).Produces<IEnumerable<ExpenseWithGroupDto>>();
        group.MapGet("{id}", Get);

        return builder;
    }

    private static async Task<Results<Ok<ExpenseWithGroupDto>, BadRequest<string>, NotFound>> Get(string id,
        IMediator mediator,
        IUserAccessor userAccessor)
    {
        var validId = Guid.TryParse(id, out var parsedId);

        if (!validId)
            return TypedResults.BadRequest("Id is not valid id");

        var result = await mediator.Send(new GetExpenseRequest(parsedId, userAccessor.GetId()));

        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<IResult> GetAll(IMediator mediator, IUserAccessor userAccessor)
        => TypedResults.Ok(await mediator.Send(new GetExpensesRequest(userAccessor.GetUsername())));

    private static async Task<Results<Ok<ExpenseDto>, BadRequest<string>>> Edit(string id, EditExpenseModel model,
        IMediator mediator,
        IUserAccessor userAccessor)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var userId = userAccessor.GetId();

        var expenseIdParsingResult = Guid.TryParse(id, out var expenseId);
        if (!expenseIdParsingResult)
            return TypedResults.BadRequest("Id is not valid guid");

        Guid.TryParse(model.ExpenseGroupId, out var expenseGroupId);

        var result =
            await mediator.Send(new EditExpenseRequest(expenseId, model.Name, model.Value, expenseGroupId, userId));

        return result is null ? TypedResults.BadRequest("Problem while updating expense") : TypedResults.Ok(result);
    }

    private static async Task<IResult> Create(AddExpenseModel model, IMediator mediator,
        IUserAccessor userAccessor)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var userId = userAccessor.GetId();

        var result =
            await mediator.Send(new AddExpenseRequest(model.Name, model.Value, userId, model.ExpenseGroupId));

        return TypedResults.Ok(result);
    }
}