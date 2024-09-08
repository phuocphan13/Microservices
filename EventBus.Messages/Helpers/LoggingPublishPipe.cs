using MassTransit;

namespace EventBus.Messages.Helpers;

public class LoggingPublishPipe<T> : IPipe<PublishContext<T>>
    where T : class
{
    public async Task Send(PublishContext<T> context)
    {
        // Store Logging.Api
        Console.WriteLine($"Publishing message: {typeof(T).Name} - {context.Message}");

        await Task.CompletedTask;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("LoggingPublishPipe");
    }

}