using Catalog.API.Services.Caches;
using Platform.Database.Redis;
using Worker.Persistance;
using Worker.Services;

namespace Catalog.API.Services.Workers;

public class RefreshCacheWorkerService : BackgroundService
{
    private const string JobName = "UpdateCache";

    private readonly IRunStateService _runStateService;
    private readonly IRedisDbFactory _redisCache;
    private readonly IServiceScopeFactory _scopeFactory;

    public RefreshCacheWorkerService(IRunStateService runStateService, IRedisDbFactory redisCache, IServiceScopeFactory scopeFactory)
    {
        _runStateService = runStateService;
        _redisCache = redisCache;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await WokerBase.RunJobAsync(async () =>
        {
            try
            {
                var isRunning = await _runStateService.IsRunningAsync(JobName, cancellationToken);

                if (!isRunning)
                {
                    return;
                }
                
                await RefreshCachedAsync(cancellationToken);
                await _runStateService.SaveJobRunningInfoAsync(JobName, true, string.Empty, cancellationToken);
            }
            catch (Exception e)
            {
                await _runStateService.SaveJobRunningInfoAsync(JobName, false, e.Message, cancellationToken);
            }
        }, TimeSpan.FromMinutes(30), cancellationToken);
    }
    
    private async Task RefreshCachedAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var productCachedService = scope.ServiceProvider.GetRequiredService<IProductCachedService>();
        
        var products = await productCachedService.GetCachedProductsAsync(cancellationToken);
        
        if (products is null)
        {
            return;
        }

        foreach (var prod in products)
        {
            if (prod.LastUpdated <= DateTime.UtcNow.AddDays(-20))
            {
                await _redisCache.RemoveAsync(prod.Id, cancellationToken);
            }
        }
    }
}