using ApiClient.Basket.Events.CheckoutEvents;
using AutoMapper;
using EventBus.Messages.Services;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutMessage>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<BasketCheckoutConsumer> _logger;
    private readonly IBasketMessageService _basketMessageService;

    public BasketCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutConsumer> logger, IBasketMessageService basketMessageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _basketMessageService = basketMessageService ?? throw new ArgumentNullException(nameof(basketMessageService));
    }

    public async Task Consume(ConsumeContext<BasketCheckoutMessage> context)
    {
        if (context.Message is null)
        {
            _logger.LogInformation("BasketCheckoutEvent is null.");
            return;
        }
        
        List<string> states = new() { OrderConstants.OrderState.Checkoutted, OrderConstants.OrderState.Accepted };
        
        var checkState = await _basketMessageService.CheckBasketStateAsync(context.Message.BasketKey, states, context.CancellationToken);

        if (checkState == true)
        {
            _logger.LogInformation("BasketCheckoutEvent already consumed. CorrelationId: {UserId}", context.Message.UserId);
            return;
        }

        var isValid = ValidateMessage(context.Message);

        if (!isValid)
        {
            return;
        }

        var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
        var result = await _mediator.Send(command);

        _logger.LogInformation("BasketCheckoutEvent consumed sucessfully. Create Order Id: {newOrderId}", result);
    }

    private bool ValidateMessage(BasketCheckoutMessage message)
    {
        bool isValid = true;

        if (message is null)
        {
            _logger.LogError("");
            return false;
        }

        if (string.IsNullOrWhiteSpace(message.BasketKey))
        {
            _logger.LogError("");
            isValid = false;
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

        if (message.TotalPrice == 0)
        {
            _logger.LogError("");
            isValid = false;
        }

        return isValid;
    }
}