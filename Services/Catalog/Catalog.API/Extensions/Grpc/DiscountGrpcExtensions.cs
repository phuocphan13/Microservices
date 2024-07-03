using ApiClient.Discount.Models.Discount;
using Discount.Grpc.Protos;

namespace Catalog.API.Extensions.Grpc;

public static class DiscountGrpcExtensions
{
    public static DiscountDetail ToDetail(this DiscountDetailModel model)
    {
        return new DiscountDetail()
        {
            Amount = model.Amount,
            Description = model.Description,
            Id = model.Id,
            CatalogCode = model.CatalogName,
            Type = (DiscountEnum)model.Type
        };
    }

    // ProductCode: A, B, C, D
    // Amount     : 1, 2, 3, 4
    // { D, 4 }, { D, 4 }, { D, 4 }, { D, 4 }
    public static List<DiscountSummary> ToListDetail(this AmountAfterDiscountResponse model)
    {
        // Variable Type: Value type, Reference type
        // Value type: int, double, decimal, char, bool, struct
        // Reference type: class, interface, delegate, array
        
        // Value type: Tham Trị
        // Reference type: Tham Chiếu
        
        // Variable: Ô nhớ, Tên, Kiểu dữ liệu
        // Different: Ô nhớ
        
        // Value: Ô nhớ chứa giá trị: 2
        // Reference: Ô nhớ chứa địa chỉ ô nhớ: 0x12345678
        
        // Giá trị = 2 ==> 0x12345678 = 2
        
        // ValueType = 2 ==> Get ValueType ==> 2
        // ReferenceType = 2 ==> Get ReferenceType ==> 0x12345678 ==> 2
        
        // 0x12345678 = 3
        
        // var listDiscounts = new List<DiscountDetail>();
        //  // discount ==> Reference Type
        //
        // foreach (var item in model.AmountDiscountResponse)
        // {
        //     var discount = new DiscountDetail
        //     {
        //         // discount.Amount = 0x12345678
        //         Amount = int.Parse(item.Amount),
        //         // discount.CatalogCode = 1x12345678
        //         CatalogCode = item.CatalogCode
        //     };
        //
        //     // listDiscounts.Add(0x12345678, 1x12345678)
        //     listDiscounts.Add(discount);
        // }

        var listDiscounts = model.AmountDiscountResponse.Select(x => new DiscountSummary()
        {
            Amount = int.Parse(x.Amount),
            CatalogCode = x.CatalogCode
        }).ToList();
        
        // discount: D, 4

        return listDiscounts;
    }
}