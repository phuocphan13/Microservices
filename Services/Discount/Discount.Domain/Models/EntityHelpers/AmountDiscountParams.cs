namespace Discount.Domain.Models.EntityHelpers;

public class AmountDiscountParams
{
    public int Type_Cate { get; set; }
    
    public string CatalogCode_Cate { get; set; } = null!;
    
    public int Type_SubCate { get; set; }
    
    public string CatalogCode_SubCate { get; set; } = null!;
    
    public int Type_Product { get; set; }
    
    public string CatalogCode_Product { get; set; } = null!;
}