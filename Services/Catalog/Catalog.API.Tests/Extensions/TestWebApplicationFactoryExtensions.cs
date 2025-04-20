using IntegrationTest.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Platform.Constants;
using Platform.Database.Entity.MongoDb;
using Platform.Database.MongoDb;

namespace Catalog.API.Tests.Extensions;

public static class TestWebApplicationFactoryExtensions
{
    public static async Task EnsureCreatedAsync<T>(this TestWebApplicationFactory<T> factory, CancellationToken cancellationToken = default) where T : class
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        EnsureMongoDbCreated(scope, cancellationToken);
    }

    public static async Task EnsureCreatedAndPopulateSingleDataAsync<TProgram, TEntity>(this TestWebApplicationFactory<TProgram> factory, TEntity entity, CancellationToken cancellationToken = default)
        where TProgram : class
        where TEntity : BaseEntity
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        EnsureMongoDbCreated(scope, cancellationToken);

        var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();

        await repository.CreateEntityAsync(entity, cancellationToken);
    }

    public static async Task EnsureCreatedAndPopulateDataAsync<TProgram, TEntity>(this TestWebApplicationFactory<TProgram> factory, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TProgram : class
        where TEntity : BaseEntity
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        EnsureMongoDbCreated(scope, cancellationToken);
        
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();

        await repository.CreateEntitiesAsync(entities, cancellationToken);
    }

    private static void EnsureMongoDbCreated(AsyncServiceScope scope, CancellationToken cancellationToken = default) 
    {
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var database = new MongoClient(configuration.GetValue<string>(DatabaseConst.ConnectionSetting.MongoDB.ConnectionString))
            .GetDatabase(configuration.GetValue<string>(DatabaseConst.ConnectionSetting.MongoDB.DatabaseName));

        database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken)
            .Wait(cancellationToken);
    }
}