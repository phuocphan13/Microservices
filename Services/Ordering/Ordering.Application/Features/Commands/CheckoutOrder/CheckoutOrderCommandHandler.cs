using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using Platform.Database.Helpers;

namespace Ordering.Application.Features.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, bool>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutOrderCommandHandler(IRepository<Order> orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);
        orderEntity.ClientName = "Sai Gon";
        orderEntity.PhoneNumber = "Sai Gon";
        orderEntity.Email = "Sai Gon";
        orderEntity.Address = "Sai Gon";
        orderEntity.CreatedBy = "Admin";
        orderEntity.CreatedDate = DateTime.UtcNow;
        orderEntity.Status = OrderStatus.Checkoutted;
        
        await _orderRepository.InsertAsync(orderEntity, cancellationToken);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    private async Task SendMail(Order order)
    {
        var email = new Email()
        {
            To = "phuoc.phan1395@gmail.com",
            Body = $"Order {order.Id} is successfully created."
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}.");
        }
    }
}
