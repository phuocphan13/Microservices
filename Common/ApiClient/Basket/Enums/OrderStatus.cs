public enum OrderStatus
{
    Nothing = 99,
    Failed = 0,
    
    Checkoutted = 1,
    Accepted = 2,
    Shipped = 3,
    Completed = 4,

    PaymentFailed = 5,
    Cancelled = 6,
    Returned = 7,
    Refunded = 8
}