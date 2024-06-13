using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Models.Discount
{
    public class SubCategoryListRequest : BaseAmountDiscountModel
    {
        public List<ProductListRequest> ProductList { get; set; } = new List<ProductListRequest>();
    }
}
