using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Discount.Models.Discount.AmountModel
{
    public class TotalAmountModel
    {
        public decimal Amount {  get; set; }
        public string CatalogCode { get; set; } = null!;
    }
}
