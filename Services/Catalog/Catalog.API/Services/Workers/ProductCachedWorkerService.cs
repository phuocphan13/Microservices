using Catalog.API.Services.Caches;
using Worker.Persistance;
using Worker.Services;

namespace Catalog.API.Services.Workers;

public class ProductCachedWorkerService : BackgroundService
{
    private const string JobName = "UpdateCache";
    
    private readonly IRunStateService _runStateService;
    private readonly IProductCachedService _productCachedService;

    public ProductCachedWorkerService(IRunStateService runStateService, IProductCachedService productCachedService)
    {
        _runStateService = runStateService;
        _productCachedService = productCachedService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await WokerBase.RunJobAsync(async () =>
        {
            try
            {
                // await RefreshProductCachedAsync(cancellationToken);
                // await _runStateService.SaveJobRunningInfoAsync(JobName, true, string.Empty, cancellationToken);
            }
            catch (Exception e)
            {
                await _runStateService.SaveJobRunningInfoAsync(JobName, false, e.Message, cancellationToken);
            }
        }, TimeSpan.FromMinutes(30), cancellationToken);
    }

    private async Task RefreshProductCachedAsync(CancellationToken cancellationToken)
    {
        await _productCachedService.RefreshCachedProductsAsync(cancellationToken);
    }
}