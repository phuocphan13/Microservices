using ApiClient.Basket.Events;
using AutoMapper;
using EventBus.Messages.Entities;
using EventBus.Messages.Exceptions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Platform.Extensions;

namespace EventBus.Messages.Services;

public interface IQueueService
{
    Task SendFanoutMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : BaseMessage, new();
    Task SendMessageAsync(object message);
    Task SendDirectMessageAsync<T>(T message, string directQueue, CancellationToken cancellationToken = default) where T : class;
}

public class QueueService : IQueueService
{
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;
    private readonly OutboxMessageDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public QueueService(IBus bus, IConfiguration configuration, OutboxMessageDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _dbContext = dbContext;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendMessageAsync(object message)
    {
        if (message is null)
        {
            throw new InvalidOperationException("Message is not allowed Null.");
        }

        await _publishEndpoint.Publish(message);
    } 
    
    public async Task SendMessageAsync<T>(T message, CancellationToken cancellationToken) 
        where T : BaseMessage, new()
    {
        if (message is null)
        {
            throw new InvalidOperationException("Message is not allowed Null.");
        }

        await _publishEndpoint.Publish(message, cancellationToken);
    }
    
    public async Task SendFanoutMessageAsync<T>(T message, CancellationToken cancellationToken) 
        where T : BaseMessage, new()
    {
        if (message is null)
        {
            throw new InvalidOperationException("Message is not allowed Null.");
        }

        var description = typeof(T).GetDescription();

        var proccess = EnumExtensions.GetEnumsByDescription<ProcessEnum>(description);

        var order = _mapper.Map<Order>(message);
        order.Proccess = proccess;

        await _dbContext.Set<Order>().AddAsync(order, cancellationToken);

        await _publishEndpoint.Publish(message, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            throw new DuplicateRegistrationException("Duplicate registration", exception);
        }
    }
    
    public async Task SendDirectMessageAsync<T>(T message, string directQueue, CancellationToken cancellationToken)
        where T: class
    {
        if (message is null)
        {
            return;
        }

        var exchangeUri = $"{_configuration.GetConfigurationValue("EventBusSettings:HostAddress")}/{directQueue}";

        var endpoint = await _bus.GetSendEndpoint(new Uri(exchangeUri));
        await endpoint.Send(message, cancellationToken);
    }
}