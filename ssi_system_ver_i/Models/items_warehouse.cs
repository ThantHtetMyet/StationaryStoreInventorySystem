using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class items_warehouse
    {
        public int items_warehouseId { get; set; }
        
        public virtual item item { get; set; }

        [Column("Reorder_Level")]
        public int reorder_level { get; set; }

        [Column("Reorder_Quantity")]
        public int reorder_quantity { get; set; }

        [Column("Unit_Of_Measuer")]
        public string unit_of_measure { get; set; }

        [Column("Stock_Balance")]
        public int stock_balance { get; set; }

        [Column("First_Supplier_Balance")]
        public int first_supplier_balance { get; set; }

        [Column("Second_Supplier_Balance")]
        public int second_supplier_balance { get; set; }

        [Column("Third_Supplier_Balance")]
        public int third_supplier_balance { get; set; }

        public items_warehouse() { }

        public items_warehouse(int reorder_level, int reorder_quantity, string unit_of_measure, int stock_balance, int first_supplier_balance, int second_supplier_balance, int third_supplier_balance)
        {
            this.reorder_level = reorder_level;
            this.reorder_quantity = reorder_quantity;
            this.unit_of_measure = unit_of_measure;
            this.stock_balance = stock_balance;
            this.first_supplier_balance = first_supplier_balance;
            this.second_supplier_balance = second_supplier_balance;
            this.third_supplier_balance = third_supplier_balance;
        }

        public items_warehouse(int reorder_level, int reorder_quantity, string unit_of_measure, int stock_balance, int first_supplier_balance, int second_supplier_balance, int third_supplier_balance,item item_obj) : this(reorder_level,reorder_quantity,unit_of_measure,stock_balance, first_supplier_balance, second_supplier_balance,third_supplier_balance)
        {
            this.item = item_obj;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}