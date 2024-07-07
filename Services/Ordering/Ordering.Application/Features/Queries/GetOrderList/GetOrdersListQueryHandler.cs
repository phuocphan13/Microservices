using AutoMapper;
using MediatR;
using Ordering.Domain.Entities;
using Platform.Database.Helpers;

namespace Ordering.Application.Features.Queries.GetOrderList;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersListQueryHandler(IRepository<Order> orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        var orderList = await _orderRepository.GetAllListAsync(x => x.UserId == request.UserId, cancellationToken);

        var orderVmList = _mapper.Map<List<OrdersVm>>(orderList);

        return orderVmList;
    }
}
