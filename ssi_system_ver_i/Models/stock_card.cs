using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class stock_card
    {
        public int stock_cardId { get; set; }

        [Column("Department_Supplier_Name")]
        public string department_supplier_name { get; set; }

        [Column("Arrival_Date")]
        public string Arrival_Date { get; set; }
        
        [Column("Sign_and_Quantity")]
        public string sign_and_quantity { get; set; }

        [Column("ITEM_ID")]
        // Table Connection
        public virtual item item_obj { get; set; }

        public stock_card() { }

        public stock_card(string department_supplier_name,string arrival_date,string sign_and_quantity)
        {
            this.department_supplier_name = department_supplier_name;
            this.Arrival_Date = arrival_date;
            this.sign_and_quantity = sign_and_quantity;
        }
        public stock_card(string department_supplier_name, string arrival_date, string sign_and_quantity,item item_obj): this(department_supplier_name, arrival_date, sign_and_quantity)
        {
            this.item_obj = item_obj;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}