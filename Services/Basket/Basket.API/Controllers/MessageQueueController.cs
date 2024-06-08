using EventBus.Messages.Events;
using EventBus.Messages.Helpers;
using EventBus.Messages.TestModel;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class MessageQueueController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;
    public readonly IConfiguration _configuration;

    public MessageQueueController(IPublishEndpoint publishEndpoint, IBus bus, IConfiguration configuration)
    {
        _publishEndpoint = publishEndpoint;
        _bus = bus;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> TestMessageQueue([FromQuery] string message, CancellationToken cancellationToken)
    {
        // await _publishEndpoint.Publish(new TestModel()
        // {
        //     Message = message
        // }, cancellationToken);
        var pipe = new LoggingPublishPipe<TestModel>();
        var exchangeUri = $"exchange:direct-queue";

        var endpoint = await _bus.GetSendEndpoint(new Uri(exchangeUri));
        await endpoint.Send(new TestModel()
        {
            Message = message
        }, cancellationToken);
        return Ok();
    }
}