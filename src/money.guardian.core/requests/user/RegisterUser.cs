using MediatR;
using Microsoft.AspNetCore.Identity;
using money.guardian.domain.entities;

namespace money.guardian.core.requests.user;

public record RegisterUserRequest(string Username, string Password) : IRequest<IdentityResult>;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, IdentityResult>
{
    private readonly UserManager<User> _userManager;

    public RegisterUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IdentityResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var user = new User { UserName = request.Username, Email = request.Username };

        return await _userManager.CreateAsync(user, request.Password);
    }
}