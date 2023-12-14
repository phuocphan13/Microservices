﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Catalog.Models.SubCategory
{
    public class SubCategorySummary
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? SubCategoryCode { get; set; }

        public string? Description { get; set; }

        public string? CategoryId { get; set; }
    }
}
