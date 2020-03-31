using ssi_system_ver_i.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.Models
{
    public class DataBaseContext : DbContext
    {
            public DataBaseContext():base("Server=DESKTOP-URDUFDP\\SQLEXPRESS;"+"Database=ssi_system_ver_i;Integrated Security=True")
            {
                Database.SetInitializer(new SSIDbInitializer<DataBaseContext>());
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);
            }

            public virtual DbSet<item> item_repository { get; set; }
            public virtual DbSet<suppliers> suppliers_repository { get; set; }
            public virtual DbSet<suppliers_warehouse> suppliers_warehouse_repository { get; set; }
            public virtual DbSet<items_warehouse> item_warehouses_repository { get; set; }
            public virtual DbSet<department> department_repository { get; set; }
            public virtual DbSet<staff> staff_repository { get; set; }
            public virtual DbSet<adjustment_voucher> adjustment_voucher_repository { get; set; }
            public virtual DbSet<orders> orders_repository { get; set; }
            public virtual DbSet<staff_representative> staff_representative_repository { get; set; }
            
            public virtual DbSet<stock_card> stock_card_repository { get; set; }
    }
}