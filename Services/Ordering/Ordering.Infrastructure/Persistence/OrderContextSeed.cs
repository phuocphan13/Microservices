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
                OrderItems = new()
                {
                    new()
                    {
                        ProductCode = "Product-Code",
                        Price = 20M,
                        Quantity = 2
                    }
                }
            }
        };
    }
}