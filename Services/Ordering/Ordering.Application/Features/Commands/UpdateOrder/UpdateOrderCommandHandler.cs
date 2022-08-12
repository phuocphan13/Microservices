using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain;

namespace Ordering.Application.Features.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<UpdateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.GetByIdAsync(request.Id);

        if (orderEntity == null)
        {
            throw new NotFoundException(nameof(request.Id), request.Id);
        }

        _mapper.Map(request, orderEntity, typeof(UpdateOrderCommand), typeof(Order));

        await _orderRepository.UpdateAsync(orderEntity);
        _logger.LogInformation($"Order {orderEntity.Id} is successfully updated.");

        return Unit.Value;
    }
}
