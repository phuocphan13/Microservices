using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Exceptions;
using Ordering.Domain;
using Platform.Database.Helpers;

namespace Ordering.Application.Features.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderCommandHandler(IRepository<Order> orderRepository, IMapper mapper, IEmailService emailService, ILogger<UpdateOrderCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (orderEntity == null)
        {
            throw new NotFoundException(nameof(request.Id), request.Id);
        }

        _mapper.Map(request, orderEntity, typeof(UpdateOrderCommand), typeof(Order));

        _orderRepository.Update(orderEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Order {orderEntity.Id} is successfully updated.");

        return Unit.Value;
    }
}
