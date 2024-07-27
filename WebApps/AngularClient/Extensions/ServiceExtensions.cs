using AngularClient.Services;
using Coupon.Grpc.Protos;

namespace AngularClient.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();

        services.AddScoped<ICouponService, CouponService>();

        services.AddScoped<IIdentityService, IdentityService>();
        return services;
    }
}