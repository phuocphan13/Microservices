using Microsoft.Extensions.Hosting;
using Worker.Persistance;
using Worker.Services;

namespace Ordering.Application.WorkerServices;

public class OrderWokerJobService : BackgroundService
{
    private const string JobName = "AcceptOrder";
    
    private readonly IBasketStateJobService _basketStateJobService;
    private readonly IRunStateService _runStateService;

    public OrderWokerJobService(IBasketStateJobService basketStateJobService, IRunStateService runStateService)
    {
        _basketStateJobService = basketStateJobService;
        _runStateService = runStateService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await WokerBase.RunJobAsync(async () =>
        {
            try
            {
                var succeed = await _basketStateJobService.ChangeStateToAcceptedAsync(cancellationToken);
                await _runStateService.SaveJobRunningInfoAsync(JobName, succeed, string.Empty, cancellationToken);
            }
            catch (Exception e)
            {
                await _runStateService.SaveJobRunningInfoAsync(JobName, false, e.Message, cancellationToken);
            }
        }, TimeSpan.FromMinutes(5), cancellationToken);
    }
}