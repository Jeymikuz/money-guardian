using MediatR;
using money.guardian.api.models.auth;
using money.guardian.api.services;
using money.guardian.core.requests.user;

namespace money.guardian.api.endpoints;

public static class AuthEndpoints
{
    private const string Prefix = "auth";
    
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(Prefix).WithOpenApi();
        group.MapPost("register", Register);
        group.MapPost("login", Login);

        return builder;
    }

    private static async Task<IResult> Register(RegisterUserModel model, IMediator mediator)
    {
        var result = await mediator.Send(new RegisterUserRequest(model.Username, model.Password));

        return !result.Succeeded ? Results.BadRequest(result.Errors) : Results.Ok();
    }

    private static async Task<IResult> Login(LoginModel model, IMediator mediator, ITokenGenerator tokenGenerator)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var claims = await mediator.Send(new AuthenticateUserRequest(model.Username, model.Password));

        if (claims is null)
            return Results.Ok(new LoginResultModel(null, false, "Incorrect username or password"));

        var token = tokenGenerator.Generate(claims);

        return Results.Ok(new LoginResultModel(token, true, null));
    }
}