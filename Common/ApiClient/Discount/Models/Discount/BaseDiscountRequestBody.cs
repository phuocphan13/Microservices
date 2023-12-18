namespace ApiClient.Discount.Models.Discount;

public class BaseDiscountRequestBody
{
    public string? CouponId { get; set; }
    
    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}