using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using IntegrationTest.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.API.Tests.Extensions;

public static class TestWebApplicationFactoryExtensions
{
    public static async Task EnsureCreatedAsync<T>(this TestWebApplicationFactory<T> factory, CancellationToken cancellationToken = default) where T : class
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        factory.EnsureMongoDbCreated(scope, cancellationToken);
    }

    public static async Task EnsureCreatedAndPopulateSingleDataAsync<TProgram, TEntity>(this TestWebApplicationFactory<TProgram> factory, TEntity entity, CancellationToken cancellationToken = default)
        where TProgram : class
        where TEntity : BaseEntity
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        factory.EnsureMongoDbCreated(scope, cancellationToken);

        var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();

        await repository.CreateEntityAsync(entity, cancellationToken);
    }

    public static async Task EnsureCreatedAndPopulateDataAsync<TProgram, TEntity>(this TestWebApplicationFactory<TProgram> factory, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TProgram : class
        where TEntity : BaseEntity
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        factory.EnsureMongoDbCreated(scope, cancellationToken);
        
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();

        await repository.CreateEntitiesAsync(entities, cancellationToken);
    }

    private static void EnsureMongoDbCreated<TProgram>(this TestWebApplicationFactory<TProgram> factory, AsyncServiceScope scope, CancellationToken cancellationToken = default) 
        where TProgram : class
    {
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var database = new MongoClient(configuration.GetValue<string>(DatabaseConst.CollectionName.ConnectionString)).GetDatabase(configuration.GetValue<string>(DatabaseConst.CollectionName.DatabaseName));

        database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken)
            .Wait(cancellationToken);
    }
}