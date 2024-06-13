using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Models.Discount.AmountModel
{
    public class SubCategoryRequestBody : CatalogItem
    {
        public List<ProductRequestBody> Products { get; set; } = new List<ProductRequestBody>();
    }
}
