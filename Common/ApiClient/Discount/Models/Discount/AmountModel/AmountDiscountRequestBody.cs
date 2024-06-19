namespace ApiClient.Discount.Models.Discount.AmountModel;

public class AmountDiscountRequestBody
{
    public List<CategoryRequestBody> Categories { get; set; } = new List<CategoryRequestBody>();
}

//categoryRequestBody
//    type
//    catalogCode
//    ListSub