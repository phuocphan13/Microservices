using Discount.Domain.Repositories;
using Discount.Domain.Repositories.Common;
using Discount.Domain.Services;
using Discount.Domain.Services.Externals;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Domain;

public static class IServiceExtensionCollection
{
    public static void AddDiscountCommonServices(this IServiceCollection services)
    {
        // services.AddScoped(typeof(IValidationResult<>), typeof(ValidationResult<>));
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<IBaseRepository, BaseRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();

        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<ICouponService, CouponService>();

        services.AddScoped<ICatalogService, CatalogService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}