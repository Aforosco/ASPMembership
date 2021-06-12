using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class SmallButtonModel
    {
        public string Action { get; set; }
        public  string Glyph { get; set; }
        public string ButtonType { get; set; }
        public string Text { get; set; }
        public int? id { get; set; }
        public int? ItemId { get; set; }
        public int? ProductId { get; set; }
        public int? SubscriptionId { get; set; }
        public string ActionParameter { get {
                var Param = new StringBuilder("?");
                if (id != null && id > 0) Param.Append(String.Format("{0}={1}&","id", id));
                if (ItemId != null && ItemId > 0) Param.Append(String.Format("{0}={1}&", "ItemId", ItemId));
                if (ProductId != null && ProductId > 0) Param.Append(String.Format("{0}={1}&", "ProductId", ProductId));
                if (SubscriptionId != null && SubscriptionId > 0) Param.Append(String.Format("{0}={1}&", "SubscriptionId", SubscriptionId));
                return Param.ToString().Substring(0, Param.Length - 1);
            } }
    }
}