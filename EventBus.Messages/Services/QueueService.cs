using EventBus.Messages.Helpers;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace EventBus.Messages.Services;

public interface IQueueService
{
    Task SendFanoutMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;

    Task SendDirectMessageAsync<T>(T message, string directQueue, CancellationToken cancellationToken = default) where T : class;
}

public class QueueService : IQueueService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;

    public QueueService(IPublishEndpoint publishEndpoint, IBus bus, IConfiguration configuration)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    public async Task SendFanoutMessageAsync<T>(T message, CancellationToken cancellationToken)
        where T: class
    {
        if (message is null)
        {
            return;
        }

        var pipe = new LoggingPublishPipe<T>();

        await _publishEndpoint.Publish(message, pipe, cancellationToken);
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