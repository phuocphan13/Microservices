using ApiClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Catalog.SubCategory.Models;

public class BaseSubCategoryResquestBody : BaseRequestBody
{
    public string? Name { get; set; }

    public string? SubCategoryCode { get; set; }

    public string? Description { get; set; }

    public string? CategoryId { get; set; }
}


