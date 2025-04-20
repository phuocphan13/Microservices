using Catalog.API.Consumers;
using Discount.Grpc.Protos;
using EventBus.Messages;
using EventBus.Messages.Entities;
using EventBus.Messages.Extensions;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Platform.Extensions;

namespace Catalog.API.Extensions.AppBuilder;

public static class ThirdPartyExtensions
{
    public static IServiceCollection AddThirdParty(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(x => x.Address = new Uri(configuration.GetConfigurationValue("GrpcSettings:DiscountUrl")));

        //MassTransit configuration
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddHostedService<MassTransitConsoleHostedService>();

        services.AddMessageOutboxCosumer(configuration, busAction: x =>
        {
            x.AddConsumer<ProductBalanceUpdateConsumer>();
        }, sagaAction: x =>
        {
            x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<OutboxMessageDbContext>();
                    r.UseSqlServer();
                });
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}