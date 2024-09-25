using Catalog.API.Common.Consts;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using IntegrationTest.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Platform.Constants;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;

namespace IntegrationTest.Common.Configurations;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly INetwork _networkBuilder = new NetworkBuilder().Build();
    
    public readonly List<IContainer> _containers = [];
    
    public WebApplicationFactory<TProgram> Instance { get; private set; } = default!;

    public new HttpClient CreateClient() => Instance.CreateClient();
    
    public Task InitializeAsync()
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Test";
        Instance = WithWebHostBuilder(builder => builder.UseEnvironment(envName));

        // Disable Testcontainers logs if CI environment
        if (envName.Equals("CI", StringComparison.OrdinalIgnoreCase))
        {
            // TestcontainersSettings. = new NullLogger<ILoggerFactory>();
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
                    builder.UseSetting(DatabaseConst.ConnectionSetting.MongoDB.ConnectionString, dbContainer.GetConnectionString());
                    // builder.UseSetting("ConnectionStrings:Db", dbContainer.GetConnectionString());
                    break;
                case PostgreSqlContainer dbContainer:
                    builder.UseSetting(DatabaseConst.ConnectionSetting.Postgres.ConnectionString, dbContainer.GetConnectionString());
                    break;
                case DockerContainer discountContainer:
                {
                    var url = $"https://{discountContainer.Hostname}:{discountContainer.GetMappedPublicPort(443)}";
                    builder.UseSetting(ApplicationConst.Discount.Url, url);
                    break;
                }
            }
        }));
    }

    public TestWebApplicationFactory<TProgram> WithMongoDbContainer()
    {
        _containers.Add(new MongoDbBuilder()
            .WithName($"test_mongodb_{Guid.NewGuid()}")
            .WithImage("mongo")
            .WithNetwork(_networkBuilder)
            .WithCleanUp(true)
            .Build());

        return this;
    }

    public TestWebApplicationFactory<TProgram> WithPostgresContainer()
    {
        _containers.Add(new PostgreSqlBuilder()
            .WithName($"test_postgres_{Guid.NewGuid()}")
            .WithUsername("admin")
            .WithPassword("admin1234")
            .WithImage("postgres")
            .WithNetwork(_networkBuilder)
            .WithCleanUp(true)
            .Build());

        return this;
    }

    public TestWebApplicationFactory<TProgram> WithDiscountContainer()
    {
        _containers.Add(new ContainerBuilder()
            .WithName($"test_discount_{Guid.NewGuid()}")
            .WithImage("docker.io/discountgrpc")
            // .WithNetwork(_networkBuilder)
            .WithExposedPort(443)
            .WithPortBinding(443, 443)
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