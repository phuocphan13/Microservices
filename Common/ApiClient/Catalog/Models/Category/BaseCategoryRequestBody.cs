﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Catalog.Models.Category
{
    public class BaseCategoryRequestBody
    {
        public string? Name { get; set; }

        public string? CategoryCode { get; set; }

        public string? Description { get; set; }
    }
}