using ApiClient.Ordering.Events;
using AutoMapper;
using EventBus.Messages.Services;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Commands.FailureOrder;

namespace Ordering.API.EventBusConsumer;

public class FailureOrderConsumer : IConsumer<FailureOrderMessage>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<FailureOrderConsumer> _logger;
    private readonly IBasketMessageService _basketMessageService;

    public FailureOrderConsumer(IMapper mapper, IMediator mediator, ILogger<FailureOrderConsumer> logger, IBasketMessageService basketMessageService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _basketMessageService = basketMessageService;
    }

    public async Task Consume(ConsumeContext<FailureOrderMessage> context)
    {
        if (context.Message is null)
        {
            _logger.LogInformation("FailureOrderEvent is null.");
            return;
        }

        List<string> states = new() { OrderConstants.OrderState.Failed };

        var checkState = await _basketMessageService.CheckBasketStateAsync(context.Message.ReceiptNumber, states, context.CancellationToken);

        if (checkState == true)
        {
            _logger.LogInformation("FailureOrderEvent already consumed. CorrelationId: {UserId}", context.Message.UserId);
            return;
        }

        var isValid = ValidateMessage(context.Message);

        if (!isValid)
        {
            return;
        }

        var command = _mapper.Map<FailureOrderCommand>(context.Message);
        var result = await _mediator.Send(command);

        _logger.LogInformation("FailureOrderEvent consumed sucessfully. Create Order Id: {newOrderId}", result);
    }

    private bool ValidateMessage(FailureOrderMessage message)
    {
        bool isValid = true;

        if (message is null)
        {
            _logger.LogError("");
            return false;
        }

        if (string.IsNullOrWhiteSpace(message.UserId))
        {
            _logger.LogError("");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(message.UserName))
        {
            _logger.LogError("");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(message.ReceiptNumber))
        {
            _logger.LogError("");
            isValid = false;
        }

        return isValid;
    }
}