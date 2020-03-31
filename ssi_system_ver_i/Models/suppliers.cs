using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class suppliers
    {
        public int suppliersId { get; set; }

        [Required]
        public string code { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string contact_name { get; set; }
        [Required]
        public int fax { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public int phone { get; set; }

        public suppliers() { }

        public suppliers(string code,string name,string contact_name,int fax,string address,int phone)
        {
            this.code = code;
            this.name = name;
            this.contact_name = contact_name;
            this.fax = fax;
            this.address = address;
            this.phone = phone;
        }
        public suppliers(int supplier_id, string code, string name, string contact_name, int fax, string address, int phone):this(code,name,contact_name,fax,address,phone)
        {
            this.suppliersId = supplier_id;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}