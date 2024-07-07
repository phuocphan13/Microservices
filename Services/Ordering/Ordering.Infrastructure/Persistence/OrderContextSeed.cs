using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (orderContext is not null && !orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", nameof(OrderContext));
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>()
        {
            new()
            {
                UserName = "swn", 
                TotalPrice = 40M,
                Address = "Sai Gon",
                ClientName = "Lucifer",
                Description = "This is the description for the first order",
                Email = "Lucifer.Moningstar@hell.com",
                PhoneNumber = "666",
                Status = OrderStatus.Checkoutted,
                CreatedBy = "Admin",
                CreatedDate = DateTime.UtcNow,
                UserId = "1412C1C6-B2F3-4F3E-B75B-E9EBDF11C4D8",
                OrderItems = new()
                {
                    new()
                    {
                        ProductCode = "ProductCode-1",
                        Price = 20M,
                        Quantity = 2
                    }
                }
            }
        };
    }
}