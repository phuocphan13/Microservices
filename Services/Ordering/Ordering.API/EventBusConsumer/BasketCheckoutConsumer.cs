using ApiClient.Basket.Events.CheckoutEvents;
using AutoMapper;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutMessage>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<BasketCheckoutConsumer> _logger;

    public BasketCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<BasketCheckoutMessage> context)
    {
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