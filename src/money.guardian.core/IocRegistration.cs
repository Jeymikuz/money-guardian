using Microsoft.Extensions.DependencyInjection;
using money.guardian.core.behaviours;
using money.guardian.core.requests.user;

namespace money.guardian.core;

public static class IocRegistration
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<RegisterUserRequest>();
            cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
        });

        return services;
    }
}