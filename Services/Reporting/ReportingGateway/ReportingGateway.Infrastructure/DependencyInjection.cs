using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Configurations.Builders;
using Platform.Database.MongoDb;

namespace ReportingGateway.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbOptions(configuration);
        services.AddEventBusSettingsOptions(configuration);
        services.AddWorkerOptions(configuration);

        services.AddSingleton(typeof(IRepository<>), typeof(Repository.Repository<>));
        
        return services;
    }
}