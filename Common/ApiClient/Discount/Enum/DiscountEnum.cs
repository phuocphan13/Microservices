using System.ComponentModel;

public enum DiscountEnum
{
    [Description("All")]
    All = 1,

    [Description("Category")]
    Category = 2,
    
    [Description("Sub-Category")]
    SubCategory = 3,

    [Description("Product")]
    Product = 4,
}