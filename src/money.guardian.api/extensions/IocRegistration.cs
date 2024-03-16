using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using money.guardian.api.services;
using money.guardian.api.settings;
using money.guardian.api.utilities;
using money.guardian.core;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.api.extensions;

public static class IocRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
              Enter 'Bearer' [space] and then your token in the text input below.
              \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Money guardian",
            });
        });
        services.AddCore();
        services.AddInfrastructure(configuration);
        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddTransient<ITokenGenerator, TokenGenerator>();
        services.AddOptions<IdentitySettings>()
            .BindConfiguration("IdentitySettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddJwtIdentity(configuration);
        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, UserAccessor>();

        return services;
    }

    private static IServiceCollection AddJwtIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = configuration["IdentitySettings:Issuer"],
                ValidAudience = configuration["IdentitySettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["IdentitySettings:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false
            };
        });
        services.AddAuthorization();

        return services;
    }
}