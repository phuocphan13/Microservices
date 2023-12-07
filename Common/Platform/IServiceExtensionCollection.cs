using Microsoft.Extensions.DependencyInjection;
using Platform.Common;

namespace Platform;

public static class IServiceExtensionCollection
{
    public static void AddPlatformCommonServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IValidationResult<>), typeof(ValidationResult<>));
    }
}