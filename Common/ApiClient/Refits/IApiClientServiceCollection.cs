using ApiClient.Refits.Discount;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ApiClient.Refits;

public static class IApiClientServiceCollection
{
    public static IServiceCollection AddDiscountApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        var discountUrl = configuration.GetSection("Microservices:DiscountApi").Value ?? throw new ArgumentNullException(nameof(configuration), "Cannot find DiscountApi configuration.");
        
        services.AddRefitClient<IDiscountApiClient>()
            .ConfigureHttpClient(builder => builder.BaseAddress = new Uri(discountUrl));
        
        return services;
    }
}