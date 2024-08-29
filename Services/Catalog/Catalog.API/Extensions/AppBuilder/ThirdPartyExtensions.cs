using Catalog.API.Consumers;
using Discount.Grpc.Protos;
using EventBus.Messages;

namespace Catalog.API.Extensions.AppBuilder;

public static class ThirdPartyExtensions
{
    public static IServiceCollection AddThirdParty(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(x => x.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]));

        services.AddMessageOutboxCosumer(configuration, busAction: x =>
        {
            x.AddConsumer<ProductBalanceUpdateConsumer>();
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}