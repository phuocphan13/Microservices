namespace ApiClient.Discount.Models.Coupon;

public class CouponSummary
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    
    public int Type { get; set; }
    
    public decimal Value { get; set; }
    
    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public bool IsActive {  get; set; }
}