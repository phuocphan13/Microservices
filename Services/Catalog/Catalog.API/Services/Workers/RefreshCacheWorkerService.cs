using Catalog.API.Services.Caches;
using Platform.Database.Redis;
using Worker.Persistance;
using Worker.Services;

namespace Catalog.API.Services.Workers;

public class RefreshCacheWorkerService : BackgroundService
{
    private const string JobName = "UpdateCache";

    private readonly IRunStateService _runStateService;
    private readonly IProductCachedService _productCachedService;
    private readonly IRedisDbFactory _redisCache;

    public RefreshCacheWorkerService(IRunStateService runStateService, IProductCachedService productCachedService, IRedisDbFactory redisCache)
    {
        _runStateService = runStateService;
        _redisCache = redisCache;
        _productCachedService = productCachedService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await WokerBase.RunJobAsync(async () =>
        {
            try
            {
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
        var products = await _productCachedService.GetCachedProductsAsync(cancellationToken);
        
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