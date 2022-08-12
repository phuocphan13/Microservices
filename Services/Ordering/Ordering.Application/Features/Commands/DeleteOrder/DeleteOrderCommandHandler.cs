using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.GetByIdAsync(request.Id);

        if (orderEntity == null)
        { 
            throw new NotFoundException(nameof(request.Id), request.Id);
        }

        await _orderRepository.DeleteAsync(orderEntity);

        return Unit.Value;
    }
}
