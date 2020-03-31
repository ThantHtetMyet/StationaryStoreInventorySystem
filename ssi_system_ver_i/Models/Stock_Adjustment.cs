using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class Stock_Adjustment
    {
        public int Stock_AdjustmentId { get; set; }
        public int quantity { get; set; }
        public string reason { get; set; }
        public virtual item item_obj { get; set; }

        public Stock_Adjustment(int quantity,string reason)
        {
            this.quantity = quantity;
            this.reason = reason;
        }

        public Stock_Adjustment(int quantity,string reason,item item_obj):this(quantity,reason)
        {
            this.item_obj = item_obj;
        }

    }
}