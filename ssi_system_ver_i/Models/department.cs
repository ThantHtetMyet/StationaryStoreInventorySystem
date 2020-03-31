using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    [Table("Department")]
    public class department
    {
        public int departmentId { get; set; }

        public string department_code { get; set; }

        public string department_name { get; set; }

        public string contact_name { get; set; }

        public int telephone_no { get; set; }

        public int fax_no { get; set; }

        public string head_name { get; set; }

        public string collection_point { get; set; }

        public string representative_name { get; set; }

        public department() { }

        public department(string department_code,string department_name,string contact_name,int telephone_no,int fax_no,string head_name,string collection_point,string representative_name)
        {
            this.department_name = department_name;
            this.department_code = department_code;
            this.contact_name = contact_name;
            this.telephone_no = telephone_no;
            this.fax_no = fax_no;
            this.head_name = head_name;
            this.collection_point = collection_point;
            this.representative_name = representative_name;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}