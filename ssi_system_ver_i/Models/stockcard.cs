using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class stockcard
    {
        public int stockcardId { get; set; }

        public DateTime date_obj { get; set; }

        public string dept_or_supplier { get; set; }

        public string quantity { get; set; }

        public int balance { get; set; }

        public stockcard(DateTime date_obj,string dept_or_supplier,string quantity,int balance)
        {
            this.date_obj = date_obj;
            this.dept_or_supplier = dept_or_supplier;
            this.quantity = quantity;
            this.balance = balance;
        }

    }
}