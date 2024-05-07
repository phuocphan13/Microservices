using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Platform.Common;
using Platform.Common.Session;

namespace Platform;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddPlatformCommonServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ISessionState, SessionState>();
        services.AddScoped(typeof(IValidationResult<>), typeof(ValidationResult<>));

        return services;
    }
}