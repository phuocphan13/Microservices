using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Platform.Database.Helpers;

namespace Ordering.Application.Features.Commands.FailureOrder;

public class FailureOrderCommandHandler : IRequestHandler<FailureOrderCommand, bool>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FailureOrderCommandHandler> _logger;

    public FailureOrderCommandHandler(IRepository<Order> orderRepository, IUnitOfWork unitOfWork, ILogger<FailureOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(FailureOrderCommand request, CancellationToken cancellationToken)
    {
        FailureOrderCommandValidator commandValidator = new();

        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError(string.Join(" ,", validationResult.Errors.Select(x => x.ErrorMessage)));
            return false;
        }
        
        var orderEntity = await _orderRepository.FirstOrDefaultAsync(x => x.ReceiptNumber == request.ReceiptNumber, cancellationToken);

        if (orderEntity is null)
        {
            return false;
        }

        orderEntity.OrderHistories.Add(new OrderHistory
        {
            CreatedDate = DateTime.Now,
            LastStatus = orderEntity.Status,
            CurrentStatus = OrderStatus.Failed,
            Order = orderEntity
        });

        orderEntity.Status = OrderStatus.Failed;

        _orderRepository.Update(orderEntity);

        return await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}