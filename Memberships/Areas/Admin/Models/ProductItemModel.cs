﻿using Memberships.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class ProductItemModel
    {
        [DisplayName("Product ID")]
        public int ProductId { get; set; }

        [DisplayName("Item ID")]
        public int ItemId { get; set; }

        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        [DisplayName("Item Title")]
        public string ItemTitle { get; set; }

        [DisplayName("Products")]
        public ICollection<Product> products { get; set; }

        [DisplayName("Items")]
        public ICollection<Item> Items { get; set; }
    }
}