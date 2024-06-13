using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.Models.AmountDiscount
{
    public class CatalogItem
    {
        public string Type { get; set; }
        public List<string> CatalogCodes { get; set; }
    }
}
