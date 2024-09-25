using ApiClient.Basket.Events;
using AutoMapper;
using EventBus.Messages.Entities;
using EventBus.Messages.Exceptions;
using Microsoft.EntityFrameworkCore;
using Platform.Extensions;

namespace EventBus.Messages.Services;

public interface IMessageService
{
    Task<Order> SumbitOutboxAsync<T>(T message, CancellationToken cancellationToken = default) where T : BaseMessage;
}

public class MessageService : IMessageService
{
    private readonly OutboxMessageDbContext _dbContext;
    private readonly IPublishService _publishService;
    private readonly IMapper _mapper;

    public MessageService(OutboxMessageDbContext dbContext, IPublishService publishService, IMapper mapper)
    {
        _dbContext = dbContext;
        _publishService = publishService;
        _mapper = mapper;
    }

    public async Task<Order> SumbitOutboxAsync<T>(T message, CancellationToken cancellationToken)
        where T: BaseMessage
    {
        var description = typeof(T).GetDescription();

        var proccess = EnumExtensions.GetEnumsByDescription<ProcessEnum>(description);
        
        var order = _mapper.Map<Order>(message);
        order.Proccess = proccess;

        await _dbContext.Set<Order>().AddAsync(order, cancellationToken);

        await _publishService.PublishMessageAsync(message, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            throw new DuplicateRegistrationException("Duplicate registration", exception);
        }

        return order;
    }
}