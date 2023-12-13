using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Catalog.Models.Catalog.Category;

public class UpdateCategoryRequestBody : BaseCategoryRequestBody
{
    public string? Id { get; set; }
}
