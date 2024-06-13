using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Models.Discount.AmountModel
{
    public class AmountDiscountResponseModel
    {
        public string Type { get; set; }
        public string CatalogCode { get; set; }
        public string Amount { get; set; }
    }
}
