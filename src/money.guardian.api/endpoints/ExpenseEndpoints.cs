using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using money.guardian.api.models.expense;
using money.guardian.api.utilities;
using money.guardian.core.common.errors;
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
        group.MapGet(string.Empty, GetAll).Produces<IEnumerable<ExpenseDto>>();
        group.MapGet("{id}", Get);
        group.MapGet("month/{month:int}", GetByMonth);

        return builder;
    }

    private static async Task<Results<Ok<IEnumerable<ExpenseDto>>, BadRequest<string>>> GetByMonth(int month,
        IMediator mediator, IUserAccessor userAccessor)
    {
        if (month is < 1 or > 12)
            return TypedResults.BadRequest("Month number should be between 1 and 12");

        var userId = userAccessor.GetId();

        var result = await mediator.Send(new GetExpensesByMonthRequest(month, userId));

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<ExpenseDto>, BadRequest<string>, NotFound>> Get(string id,
        IMediator mediator,
        IUserAccessor userAccessor)
    {
        var validId = Guid.TryParse(id, out var parsedId);

        if (!validId)
            return TypedResults.BadRequest("Id is not valid id");

        var result = await mediator.Send(new GetExpenseRequest(parsedId, userAccessor.GetId()));

        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result.Value);
    }

    private static async Task<IResult> GetAll(IMediator mediator, IUserAccessor userAccessor)
    {
        var result = await mediator.Send(new GetExpensesRequest(userAccessor.GetUsername()));

        if (!result.IsError)
        {
            return TypedResults.Ok(result.Value);
        }

        return result.Error is UnAuthorizedError ? Results.Forbid() : TypedResults.Problem();
    }

    private static async Task<Results<Ok<ExpenseDto>, BadRequest<string>>> Edit(string id, EditExpenseModel model,
        IMediator mediator,
        IUserAccessor userAccessor)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var userId = userAccessor.GetId();

        var expenseIdParsingResult = Guid.TryParse(id, out var expenseId);
        if (!expenseIdParsingResult)
            return TypedResults.BadRequest("Id is not valid guid");

        var result =
            await mediator.Send(
                new EditExpenseRequest(expenseId, model.Name, model.Value, model.ExpenseGroupId, userId));

        return result.IsError
            ? TypedResults.BadRequest("Problem while updating expense")
            : TypedResults.Ok(result.Value);
    }

    private static async Task<IResult> Create(AddExpenseModel model, IMediator mediator,
        IUserAccessor userAccessor)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var userId = userAccessor.GetId();

        var result =
            await mediator.Send(new AddExpenseRequest(model.Name, model.Value, userId, model.ExpenseGroupId));

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.Error.Message);
        }

        return TypedResults.Ok(result.Value);
    }
}