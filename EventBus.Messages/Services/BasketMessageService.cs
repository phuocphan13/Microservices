using EventBus.Messages.Entities;
using EventBus.Messages.StateMachine.Basket;
using Microsoft.EntityFrameworkCore;

namespace EventBus.Messages.Services;

public interface IBasketMessageService
{
    Task<bool?> CheckBasketStateAsync(string receiptNumber, List<string> states, CancellationToken cancellationToken = default);
}

public class BasketMessageService : IBasketMessageService
{
    private readonly OutboxMessageDbContext _dbContext;

    public BasketMessageService(OutboxMessageDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool?> CheckBasketStateAsync(string receiptNumber, List<string> states, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Set<OrderState>().AnyAsync(x => x.CorrelationId == Guid.Parse(receiptNumber) && states.Contains(x.CurrentState), cancellationToken);

        return isAny;
    }
}