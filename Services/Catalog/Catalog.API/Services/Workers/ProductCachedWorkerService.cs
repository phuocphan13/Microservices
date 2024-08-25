using Catalog.API.Services.Caches;
using Worker.Persistance;
using Worker.Services;

namespace Catalog.API.Services.Workers;

public class ProductCachedWorkerService : BackgroundService
{
    private const string JobName = "UpdateCache";
    
    private readonly IRunStateService _runStateService;
    private readonly IServiceScopeFactory _scopeFactory;

    public ProductCachedWorkerService(IRunStateService runStateService, IServiceScopeFactory scopeFactory)
    {
        _runStateService = runStateService;
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
                
                await RefreshProductCachedAsync(cancellationToken);
                await _runStateService.SaveJobRunningInfoAsync(JobName, true, string.Empty, cancellationToken);
            }
            catch (Exception e)
            {
                await _runStateService.SaveJobRunningInfoAsync(JobName, false, e.Message, cancellationToken);
            }
        }, TimeSpan.FromMinutes(30), cancellationToken);
    }

    private async Task RefreshProductCachedAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var productCachedService = scope.ServiceProvider.GetRequiredService<IProductCachedService>();
        
        await productCachedService.RefreshCachedProductsAsync(cancellationToken);
    }
}