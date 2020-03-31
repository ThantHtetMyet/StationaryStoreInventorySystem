using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class suppliers_warehouse
    {
        public int suppliers_warehouseId { get; set; }

        public int stock_balance { get; set; }

        public int price { get; set; }

        public int original_balance { get; set; }

        public virtual suppliers suppliers_warehouse_plus_supplier { get; set; }

        public virtual item suppliers_warehouse_plus_item { get; set; }

        public suppliers_warehouse() { }

        public suppliers_warehouse(int stock_balance, int original_balance, int price)
        {
            this.stock_balance = stock_balance;
            this.original_balance = original_balance;
            this.price = price;
        }
        
        /*
        public suppliers_warehouse(int stock_balance, int original_balance, int price,int forkey_supplier,int forkey_item)
        {
            this.foreignkey_supplier = forkey_supplier;
            this.foreignkey_item = forkey_item;
            this.stock_balance = stock_balance;
            this.original_balance = original_balance;
            this.price = price;
        }
        */
        public suppliers_warehouse(int stock_balance, int original_balance, int price, suppliers suppliers_warehouse_plus_supplier, item suppliers_warehouse_plus_item) :this(stock_balance, original_balance, price)
        {
            this.suppliers_warehouse_plus_supplier = suppliers_warehouse_plus_supplier;
            this.suppliers_warehouse_plus_item = suppliers_warehouse_plus_item;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}