using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Entities;
using Platform.Database.Helpers;

namespace Ordering.Application.WorkerServices;

public interface IBasketStateJobService
{
    Task<bool> ChangeStateToAcceptedAsync(CancellationToken cancellationToken = default);
}

public class BasketStateJobService : IBasketStateJobService
{
    private readonly IServiceScopeFactory scopeFactory;

    public BasketStateJobService(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    public async Task<bool> ChangeStateToAcceptedAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var reposity = scope.ServiceProvider.GetRequiredService<IRepository<OrderHistory>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var timeCheck = DateTime.UtcNow.AddMinutes(-10);
        
        var orderHistories = await reposity.GetAllListAsync(x => x.CurrentStatus == OrderStatus.Checkoutted && x.CreatedDate <= timeCheck, cancellationToken);

        var histories = new List<OrderHistory>();
        foreach (var history in orderHistories)
        {
            histories.Add(new()
            {
                OrderId = history.OrderId,
                LastStatus = history.CurrentStatus,
                CurrentStatus = OrderStatus.Accepted
            });
        }

        await reposity.InsertAsync(histories, cancellationToken);
        return await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}