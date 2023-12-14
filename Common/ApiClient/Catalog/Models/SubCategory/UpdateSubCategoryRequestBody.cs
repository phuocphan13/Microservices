using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Catalog.Models.SubCategory
{
    public class UpdateSubCategoryRequestBody : BaseSubCategoryResquestBody
    {
        public string? Id { get; set; }
    }
}
