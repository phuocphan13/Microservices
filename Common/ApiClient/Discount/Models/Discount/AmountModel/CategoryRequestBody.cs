using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Models.Discount.AmountModel
{
    public class CategoryRequestBody : CatalogItem
    {
        public List<SubCategoryRequestBody> SubCategories { get; set; }  = new List<SubCategoryRequestBody>();
    }
}
