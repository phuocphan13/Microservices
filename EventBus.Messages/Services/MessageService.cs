using ApiClient.Basket.Events;
using AutoMapper;
using EventBus.Messages.Entities;
using EventBus.Messages.Exceptions;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EventBus.Messages.Services;

public interface IMessageService
{
    Task<Basket> SumbitOutboxAsync<T>(T message, CancellationToken cancellationToken = default) where T : BaseMessage;
    Task<bool?> CheckStateMessageAsync(Guid correlationId, State state, CancellationToken cancellationToken = default);
}

public class MessageService : IMessageService
{
    private readonly MessageDbContext _dbContext;
    private readonly IPublishService _publishService;
    private readonly IMapper _mapper;

    public MessageService(MessageDbContext dbContext, IPublishService publishService, IMapper mapper)
    {
        _dbContext = dbContext;
        _publishService = publishService;
        _mapper = mapper;
    }

    public async Task<Basket> SumbitOutboxAsync<T>(T message, CancellationToken cancellationToken)
        where T: BaseMessage
    {
        var basket = _mapper.Map<Basket>(message);

        await _dbContext.Set<Basket>().AddAsync(basket, cancellationToken);

        await _publishService.PublishMessageAsync(message, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
            // when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new DuplicateRegistrationException("Duplicate registration", exception);
        }

        return basket;
    }
    
    public async Task<bool?> CheckStateMessageAsync(Guid correlationId, State state, CancellationToken cancellationToken)
    {
        var message = await _dbContext.Set<BasketState>().FirstOrDefaultAsync(x => x.CorrelationId == correlationId, cancellationToken);

        if (message is null)
        {
            return null;
        }

        return message.CurrentState == state.ToString();
    }
}