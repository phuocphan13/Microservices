using Microsoft.Extensions.Logging;
using Ordering.Infrastruture.Persistence;
using Platform.Database.Helpers;

namespace Ordering.Infrastruture.Database;

public class UnitOfWork : UnitOfWorkBase<OrderContext>
{
    public UnitOfWork(OrderContext authenContext, ILogger<UnitOfWork> logger) : base(authenContext, logger)
    {
    }
}