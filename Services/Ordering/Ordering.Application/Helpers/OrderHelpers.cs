using Ordering.Domain.Entities;

namespace Ordering.Application.Helpers;

public static class OrderHelpers
{
    public static void AddAuditInfo(this Order order)
    {
        order.ClientName = "Sai Gon";
        order.PhoneNumber = "Sai Gon";
        order.Email = "Sai Gon";
        order.Address = "Sai Gon";
        order.CreatedBy = "Admin";
        order.CreatedDate = DateTime.UtcNow;
        order.Status = OrderStatus.Checkoutted;
    }
}