using Memberships.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class SubscriptionProductModel
    {
        [DisplayName("Product ID")]
        public int ProductId { get; set; }

        [DisplayName(" Subscription ID")]
        public int SubscriptionId { get; set; }

        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        [DisplayName("Subscription Title")]
        public string SubscriptionTitle { get; set; }

        [DisplayName("Products")]
        public ICollection<Product> products { get; set; }

        [DisplayName("Subscription")]
        public ICollection<Subscription> subscriptions { get; set; }
    }
}