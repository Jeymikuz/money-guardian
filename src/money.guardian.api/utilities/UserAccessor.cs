using System.Security.Claims;

namespace money.guardian.api.utilities;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername()
    {
        var context = _httpContextAccessor.HttpContext;

        return context?.User.FindFirstValue(ClaimTypes.Name);
    }

    public string GetId()
    {
        var context = _httpContextAccessor.HttpContext;

        return context?.User.FindFirstValue(ClaimTypes.Sid);
    }
}