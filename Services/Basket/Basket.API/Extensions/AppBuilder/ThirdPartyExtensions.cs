using Discount.Grpc.Protos;
using EventBus.Messages;
using Platform.Extensions;

namespace Basket.API.Extensions.AppBuilder;

public static class ThirdPartyExtensions
{
    public static IServiceCollection AddThirdParty(this IServiceCollection services, IConfiguration configuration)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(x => x.Address = new Uri(configuration.GetConfigurationValue("GrpcSettings:DiscountUrl")));

        services.AddMessageOutbox(configuration);
        
        services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = configuration.GetConfigurationValue("CacheSettings:ConnectionString");
        });
        
        return services;
    }
}