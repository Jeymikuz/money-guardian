using Microsoft.Extensions.DependencyInjection;
using money.guardian.core.requests.user;

namespace money.guardian.core;

public static class IocRegistration
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<RegisterUserRequest>());
        
        return services;
    }
}