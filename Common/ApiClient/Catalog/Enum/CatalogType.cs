using System.ComponentModel;

public enum CatalogType
{
    Unknown = 0,
    
    [Description("Category")] 
    Category = 1,

    [Description("SubCategory")] 
    SubCategory = 2,

    [Description("ProductName")] 
    Product = 3
}