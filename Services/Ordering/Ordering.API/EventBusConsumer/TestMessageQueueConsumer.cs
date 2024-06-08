using EventBus.Messages.TestModel;
using MassTransit;

namespace Ordering.API.EventBusConsumer;

public class TestMessageQueueConsumer: IConsumer<TestModel>
{
    public async Task Consume(ConsumeContext<TestModel> context)
    {
        Console.WriteLine("Received message: " + context.Message.Message);
        await Task.CompletedTask;
    }
}