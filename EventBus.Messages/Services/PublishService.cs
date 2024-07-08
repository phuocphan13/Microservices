using ApiClient.Basket.Events;
using MassTransit;

namespace EventBus.Messages.Services;

public interface IPublishService
{
    Task PublishMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : BaseMessage;
}

public class PublishService : IPublishService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishMessageAsync<T>(T message, CancellationToken cancellationToken)
        where T : BaseMessage
    {
        await _publishEndpoint.Publish(message, cancellationToken);
    }
}