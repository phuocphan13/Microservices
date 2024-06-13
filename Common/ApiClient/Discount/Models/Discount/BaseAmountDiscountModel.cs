using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Models.Discount
{
    public class BaseAmountDiscountModel
    {
        public DiscountEnum? Type { get; set; }
        public string? CatalogCode { get; set; }
    }
}
