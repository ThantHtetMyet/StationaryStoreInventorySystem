using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class adjustment_voucher
    {
        public int adjustment_voucherId { get; set; }
        public int item_quantity { get; set; }
        public string reason { get; set; }
        public string record_date { get; set; }
        public virtual item item_obj { get; set; }

        public string status { get; set; }

        public adjustment_voucher() { }

        public adjustment_voucher(item item_obj,int item_quantity,string reason,string record_date,string status)
        {
            this.item_obj = item_obj;
            this.item_quantity = item_quantity;
            this.reason = reason;
            this.record_date = record_date;
            this.status = status;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}