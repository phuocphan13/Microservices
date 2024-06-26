using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.Models.EntityHelpers
{
    public class AmountDiscountParams
    {
        public int Type_Cate { get; set; }
        public string CatalogCode_Cate { get; set; }
        public int Type_SubCate { get; set; }
        public string CatalogCode_SubCate { get; set; }
        public int Type_Product { get; set; }
        public string CatalogCode_Product { get; set; }
    }
}
