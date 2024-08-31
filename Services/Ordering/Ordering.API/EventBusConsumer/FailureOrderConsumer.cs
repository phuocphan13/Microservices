using ApiClient.Ordering.Events;
using MassTransit;

namespace Ordering.API.EventBusConsumer;

public class FailureOrderConsumer : IConsumer<FailureOrderMessage>
{
    public async Task Consume(ConsumeContext<FailureOrderMessage> context)
    {
    }
}