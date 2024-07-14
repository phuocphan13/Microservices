using ApiClient.Basket.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace EventBus.Messages.Services;

public interface IQueueService
{
    Task SendFanoutMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : BaseMessage, new();
    Task SendDirectMessageAsync<T>(T message, string directQueue, CancellationToken cancellationToken = default) where T : class;
}

public class QueueService : IQueueService
{
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;
    private readonly IMessageService _messageService;

    public QueueService(IBus bus, IConfiguration configuration, IMessageService messageService)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }
    
    public async Task SendFanoutMessageAsync<T>(T message, CancellationToken cancellationToken) 
        where T : BaseMessage, new()
    {
        if (message is null)
        {
            throw new InvalidOperationException("Message is not allowed Null.");
        }
        
        await _messageService.SumbitOutboxAsync(message, cancellationToken);
    }
    
    public async Task SendDirectMessageAsync<T>(T message, string directQueue, CancellationToken cancellationToken)
        where T: class
    {
        if (message is null)
        {
            return;
        }

        var exchangeUri = $"{_configuration["EventBusSettings:HostAddress"]}/{directQueue}";

        var endpoint = await _bus.GetSendEndpoint(new Uri(exchangeUri));
        await endpoint.Send(message, cancellationToken);
    }
}