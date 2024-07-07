using MediatR;

namespace Ordering.Application.Features.Queries.GetOrderList;

public class GetOrdersListQuery : IRequest<List<OrdersVm>>
{
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }

    public GetOrdersListQuery(string userName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
    }
}
