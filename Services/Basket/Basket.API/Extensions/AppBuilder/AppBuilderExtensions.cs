using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.API.Services;

namespace Basket.API.Extensions.AppBuilder;

public static class AppBuilderExtensions
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();
        services.AddScoped<IBasketService, BasketService>();
            
        return services;
    }
}