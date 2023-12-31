using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using IntegrationTest.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Testcontainers.MongoDb;

namespace IntegrationTest.Common.Configurations;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    public readonly List<IContainer> _containers = new();
    
    public WebApplicationFactory<TProgram> Instance { get; private set; } = default!;
    
    public new HttpClient CreateClient() => Instance.CreateClient();
    
    public Task InitializeAsync()
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Test";
        Instance = WithWebHostBuilder(builder => builder.UseEnvironment(envName));

        // Disable Testcontainers logs if CI environment
        if (envName.Equals("CI", StringComparison.OrdinalIgnoreCase))
        {
            TestcontainersSettings.Logger = new NullLogger<ILoggerFactory>();
        }

        return Task.CompletedTask;
    }

    public async Task StartContainersAsync(CancellationToken cancellationToken = default)
    {
        // Do nothing if no containers
        if (_containers.Count == 0)
        {
            return;
        }

        // Start all containers
        await Task.WhenAll(_containers.Select(container => container.StartWithWaitAndRetryAsync(cancellationToken: cancellationToken)));

        // Update Settings for each container
        Instance = _containers.Aggregate(this as WebApplicationFactory<TProgram>, (current, container) => current.WithWebHostBuilder(builder =>
        {
            switch (container)
            {
                case MongoDbContainer dbContainer:
                    builder.UseSetting("ConnectionStrings:Db", dbContainer.GetConnectionString());
                    break;
                // case PostgreSqlContainer dbContainer:
                //     builder.UseSetting("ConnectionStrings:Db", dbContainer.GetConnectionString());
                //     break;
                //
                // case RedisContainer cacheContainer:
                //     builder.UseSetting("ConnectionStrings:Cache", cacheContainer.GetConnectionString());
                //     break;
            }
        }));
    }

    public TestWebApplicationFactory<TProgram> WithMongoDbContainer()
    {
        _containers.Add(new MongoDbBuilder()
            .WithName($"test_mongodb_{Guid.NewGuid()}")
            .WithImage("mongo")
            .WithCleanUp(true)
            .Build());

        return this;
    }

    public async Task StopContainersAsync()
    {
        // Do nothing if no containers
        if (_containers.Count == 0)
        {
            return;
        }

        await Task.WhenAll(_containers.Select(container => container.DisposeAsync().AsTask()))
            .ContinueWith(async _ => await Instance.DisposeAsync())
            .ContinueWith(async _ => await InitializeAsync());

        _containers.Clear();
    }

    public new Task DisposeAsync()
    {
        return Task
            .WhenAll(_containers.Select(container => container.DisposeAsync().AsTask()))
            .ContinueWith(async _ => await base.DisposeAsync());
    }
}