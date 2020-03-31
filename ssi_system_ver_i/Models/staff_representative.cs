using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class staff_representative
    {
        public int staff_representativeId { get; set; }

        public string start_date { get; set; }
        public string end_date { get; set; }
        public staff representative_staff_obj { get; set; }

        public string position { get; set; }

        public staff_representative()
        { }

        public staff_representative(string start_date, string end_date,staff staff_obj,string position)
        {
            this.position = position;
            this.start_date = start_date;
            this.end_date = end_date;
            this.representative_staff_obj = staff_obj;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}