using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class item
    {
        public int itemId { get; set; }

        [Required(ErrorMessage = "Please enter code")]
        [Column("Code")]
        public string item_code { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        [Column("Description")]
        public string item_description { get; set; }

        [Required(ErrorMessage ="Please enter category")]
        [Column("Cateogry")]
        public string category { get; set; }

        [Column("Unit_Price")]
        public int unit_price { get; set; }

        public item(string itemcode,string itemDescription,string category,int unit_price)
        {
            this.item_code = itemcode;
            this.item_description = itemDescription;
            this.category = category;
            this.unit_price = unit_price;
        }
        public item(int itemid, string itemcode, string itemDescription,int unit_price, string category):this(itemcode,itemDescription,category,unit_price)
        {
            this.itemId = itemid;
        }

        public item()
        { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}