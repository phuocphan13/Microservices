using Ordering.Domain;

namespace Ordering.Application.Contracts.Persistence;

public interface IOrderRepository : IAsyncRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
}
