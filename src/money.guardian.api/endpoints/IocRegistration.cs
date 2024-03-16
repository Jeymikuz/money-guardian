namespace money.guardian.api.endpoints;

public static class IocRegistration
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapVersion("v1");
        group.MapAuth();
        group.MapExpense();
        group.MapExpenseGroup();

        return builder;
    }

    private static RouteGroupBuilder MapVersion(this IEndpointRouteBuilder builder, string version)
    {
        return builder.MapGroup(version);
    }
}