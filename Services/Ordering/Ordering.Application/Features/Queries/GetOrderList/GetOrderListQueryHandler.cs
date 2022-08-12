using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Queries.GetOrderList;

public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrdersVm>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<OrdersVm>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
        var orderList = await _orderRepository.GetOrdersByUserName(request.UserName!);

        var orderVmList = _mapper.Map<List<OrdersVm>>(orderList);

        return orderVmList;
    }
}
