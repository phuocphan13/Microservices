using ApiClient.Basket.Events.CheckoutEvents;
using EventBus.Messages.Entities;
using EventBus.Messages.StateMachine.Basket;
using Microsoft.EntityFrameworkCore;

namespace EventBus.Messages.Services;

public interface IBasketMessageService
{
    Task<bool?> CheckBasketStateAsync(BasketCheckoutMessage message, List<string> states, CancellationToken cancellationToken = default);
}

public class BasketMessageService : IBasketMessageService
{
    private readonly OrderMessageDbContext _dbContext;

    public BasketMessageService(OrderMessageDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool?> CheckBasketStateAsync(BasketCheckoutMessage message, List<string> states, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Set<OrderState>().AnyAsync(x => x.CorrelationId == Guid.Parse(message.BasketKey) && states.Contains(x.CurrentState), cancellationToken);

        return isAny;
    }
}