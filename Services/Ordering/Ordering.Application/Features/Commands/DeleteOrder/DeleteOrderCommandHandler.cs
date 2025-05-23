﻿using MediatR;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using Platform.Database.Helpers;

namespace Ordering.Application.Features.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IRepository<Order> orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (orderEntity == null)
        { 
            throw new NotFoundException(nameof(request.Id), request.Id);
        }

        _orderRepository.Delete(orderEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
