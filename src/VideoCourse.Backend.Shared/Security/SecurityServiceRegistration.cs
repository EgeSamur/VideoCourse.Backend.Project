using Microsoft.Extensions.DependencyInjection;
using VideoCourse.Backend.Shared.Security.JWT;

namespace VideoCourse.Backend.Shared.Security;

/// <summary>
/// Provides extension methods for registering security services.
/// </summary>
public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, JwtHelper>();
        return services;
    }
}