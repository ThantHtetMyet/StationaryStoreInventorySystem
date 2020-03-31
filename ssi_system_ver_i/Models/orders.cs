using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class orders
    {
        public int ordersId { get; set; }
        public int proposed_quantity { get; set; }
        public int actual_delivered_quantity_by_clerk { get; set; }
        public int actual_received_quantity_by_representative { get; set; }
        public string order_status { get; set; }
        public string start_order_date { get; set; }
        public string delivered_order_date { get; set; }
        public virtual staff staff_obj { get; set; }
        public virtual item item_obj { get; set; }

        public orders() { }

        public orders(staff staff_obj, item item_obj, int proposed_quantity, string start_order_date)
        {
            this.start_order_date = start_order_date;
            this.staff_obj = staff_obj;
            this.item_obj = item_obj;
            this.proposed_quantity = proposed_quantity;
            this.actual_delivered_quantity_by_clerk = 0;
            this.actual_received_quantity_by_representative = 0;
            this.order_status = "pending";
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}