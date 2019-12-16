﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.models;

namespace MyShop.Core.ViewModels
{
    public class ProductManagerViewModel
    {
        public Product product { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
