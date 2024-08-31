using ApiClient.Catalog.Product.Events;
using ApiClient.Catalog.ProductHistory.Models;
using ApiClient.Ordering.Events;
using AutoMapper;
using EventBus.Messages.Services;
using MediatR;
using Ordering.Application.Helpers;
using Ordering.Domain.Entities;
using Platform.Database.Helpers;

namespace Ordering.Application.Features.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, bool>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueueService _queueService;

    public CheckoutOrderCommandHandler(IRepository<Order> orderRepository, IMapper mapper, IUnitOfWork unitOfWork, IQueueService queueService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _queueService = queueService;
    }

    public async Task<bool> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);
        orderEntity.AddAuditInfo();
        
        orderEntity.OrderHistories = new List<OrderHistory>()
        {
            new()
            {
                CreatedDate = DateTime.Now,
                LastStatus = OrderStatus.Nothing,
                CurrentStatus = OrderStatus.Checkoutted,
                Order = orderEntity
            }
        };
        
        await _orderRepository.InsertAsync(orderEntity, cancellationToken);

        var result = false;
        try
        {
            result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _queueService.SendFanoutMessageAsync(new FailureOrderMessage()
            {
                ReceiptNumber = orderEntity.ReceiptNumber,
                UserId = request.UserId,
                UserName = request.UserName,
            }, cancellationToken);
        }

        if (result)
        {
            await _queueService.SendFanoutMessageAsync(new ProductBalanceUpdateMessage()
            {
                ReceiptNumber = orderEntity.ReceiptNumber,
                UserId = request.UserId,
                UserName = request.UserName,
                Products = orderEntity.OrderItems.Select(x => new ReduceProductBalanceRequestBody()
                {
                    ProductCode = x.ProductCode!,
                    Quantity = x.Quantity
                })
            }, cancellationToken);
        }

        return result;
    }
}
