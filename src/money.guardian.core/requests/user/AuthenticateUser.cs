using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using money.guardian.domain.entities;

namespace money.guardian.core.requests.user;

public record AuthenticateUserRequest(string Username, string Password) : IRequest<IEnumerable<Claim>>;

public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserRequest, IEnumerable<Claim>>
{
    private readonly UserManager<User> _userManager;

    public AuthenticateUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IEnumerable<Claim>> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            return null;

        var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        var claims = await _userManager.GetClaimsAsync(user);
        claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
        claims.Add(new Claim(ClaimTypes.Sid, user.Id));

        return validPassword ? claims : null;
    }
}