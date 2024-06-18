using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Persistence;
using Platform.Database.Helpers;

namespace Ordering.Infrastructure.Database;

public class UnitOfWork : UnitOfWorkBase<OrderContext>
{
    public UnitOfWork(OrderContext orderContext, ILogger<UnitOfWork> logger) : base(orderContext, logger)
    {
    }
}