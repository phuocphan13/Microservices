using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Catalog.Models.Catalog.Category;

public class CategorySummary
{
    public string? Id { get;set; }

    public string? Name { get; set; }

    public string? CategoryCode { get; set; }

    public string? Description { get; set; }
}
