using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Models.Discount.AmountModel
{
    public class AmountDiscountRepositoryModel
    {
        public string Type { get; set; } 
        public List<string> CatalogCodes { get; set; } = new List<string>();
    }
}
