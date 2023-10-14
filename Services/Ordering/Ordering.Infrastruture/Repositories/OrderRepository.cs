using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain;
using Ordering.Infrastruture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastruture.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        var orderList = await _dbContext.Orders.Where(x => string.Equals(x.UserName, userName)).ToListAsync();

        return orderList;
    }
}
