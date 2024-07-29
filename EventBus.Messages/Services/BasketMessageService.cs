using ApiClient.Basket.Events.CheckoutEvents;
using EventBus.Messages.Entities;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EventBus.Messages.Services;

public interface IBasketMessageService
{
    Task<bool?> CheckBasketStateAsync(BasketCheckoutMessage message, string state, CancellationToken cancellationToken = default);
}

public class BasketMessageService : IBasketMessageService
{
    private readonly MessageDbContext _dbContext;

    public BasketMessageService(MessageDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool?> CheckBasketStateAsync(BasketCheckoutMessage message, string state, CancellationToken cancellationToken)
    {
        var basket = await _dbContext.Set<BasketState>().FirstOrDefaultAsync(x => x.CorrelationId == Guid.Parse(message.UserId), cancellationToken);

        if (basket is null)
        {
            return null;
        }

        return basket.CurrentState == state.ToString() && basket.TotalPrice == message.TotalPrice;
    }
}