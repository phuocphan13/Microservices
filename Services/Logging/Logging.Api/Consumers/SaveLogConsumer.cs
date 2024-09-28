using ApiClient.Logging.Events;
using Logging.Services;
using MassTransit;

namespace Logging.Consumers;

public class SaveLogConsumer : IConsumer<SaveLogsMessage>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SaveLogConsumer> _logger;

    public SaveLogConsumer(IServiceProvider serviceProvider, ILogger<SaveLogConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SaveLogsMessage> context)
    {
        var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILogService>();
        
        var result = await service.CreateLogsAsync(context.Message.RequestBodies, context.CancellationToken);

        if (result)
        {
            _logger.LogInformation("Save logs successfully.");
        }
        else
        {
            _logger.LogError("Save logs failed.");
        }
    }
}