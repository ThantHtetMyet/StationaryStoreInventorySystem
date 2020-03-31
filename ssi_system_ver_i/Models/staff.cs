using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class staff
    {
        public int staffId { get; set; }

        [Column("Staff_Name")]
        public string name { get; set; }
        [Column("AccountId")]
        public string email { get; set; }

        [Column("Password")]
        public string password { get; set; }

        [Column("Age")]
        public int age { get; set; }

        [Column("Gender")]
        public string gender { get; set; }

        [Column("Position")]
        public string position { get; set; }

        public virtual department department_obj { get; set; }

        public staff(string name,string email,string password,int age,string gender,string position)
        {
            this.email = email;
            this.password = password;
            this.position = position;
            this.name = name;
            this.age = age;
            this.gender = gender;
        }

        public staff(string name,string email,string password,int age,string gender,string position,department department_obj) : this(name,email,password,age,gender,position)
        {
            this.department_obj = department_obj;
        }

        public staff() { }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}