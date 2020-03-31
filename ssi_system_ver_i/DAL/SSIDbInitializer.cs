using ssi_system_ver_i.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ssi_system_ver_i.DAL
{
    public class SSIDbInitializer<T>: DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            // ITEM INITIALIZATION
            string[] item_code = new string[] { "C001", "C002", "C003", "E001", "E002", "E003", "F020", "F021", "F022", "H011", "H012", "H013", "P010", "P011", "P012", "R002", "R001", "S100", "S040", "S101", "T001", "T002", "T003" };
            string[] category = new string[] { "Clip", "Clip", "Clip", "Envelope", "Envelope", "Envelope", "File", "File", "File", "Pen", "Pen", "Pen", "Pad", "Pad", "Pad", "Ruler", "Ruler", "Scissors", "Tape", "Tape", "Tracks", "Tacks", "Tacks" };
            string[] description = new string[] { "Clips Double 1\"", "Clips Double 2\"", "Clips Double 3/4 \"", "Envelope Brown 3*6", "Envelope Brown 4*7", "Envelope Brown 5*7", "File Separator", "File-Blue Plain", "File-Blue with Logo", "Highlighter Blue", "Highlighter Green", "Highlighter Pink", "Hole Puncher 2 holes", "Hole Puncher 3 holes", "Hole Puncher Adjustable", "Ruler 12\"", "Ruler 6\"", "Scissors", "Scotch Tape", "Scotch Tape Dispenser", "Thumb Tacks Large", "Thumb Tacks Medium", "Thumb Tacks Small" };
            Random unit_price_random = new Random();

            List<item> item_list = new List<item>();
            for (int i=0;i<item_code.Length;i++)
            {
                item item_obj = new item();
                item_obj.item_code = item_code[i];
                item_obj.item_description = description[i];
                item_obj.category = category[i];
                item_obj.unit_price = unit_price_random.Next(1, 20);
                item_list.Add(item_obj);

                context.item_repository.Add(item_obj);
                context.SaveChanges();
            }
            
            // SUPPLIER INITIALIZATION
            string[] supplier_code = new string[] { "ALPA", "CHEP", "BANE", "OMEG" };
            string[] supplier_name = new string[] { "ALPHA Office Supplies", "Cheap Stationer", "BANE SHOP", "OMEGA Stationery Supplier" };
            string[] contact_name = new string[] { "Ms Irene Tan", "Mr Soh Kway Koh", "Mr Loh Ah Pek", "Mr Ronnie Ho" };
            int[] phone_no = new int[] { 46119928, 3543234, 4781234, 7671233 };
            int[] fax = new int[] { 4612238, 4742434, 4792434, 7671234 };
            string[] address = new string[] { " Blk 1128, Ang Mo Kio Industrial Park,#02-1108 Ang Mo Kio Street 62,Singapore 622262", "Blk 34, Clementi Road,#07-02 Ban Ban Soh Building,Singapore 110525", " Blk 124, Alexandra Road,#03-04 Banes Building,Singapore 550315", "Blk 11, Hillview Avenue,#03-04,Singapore 679036" };
            List<suppliers> suppliers_lis = new List<suppliers>();

            for(int i=0;i<supplier_code.Length;i++)
            {
                suppliers supplier_obj = new suppliers(supplier_code[i],supplier_name[i],contact_name[i],fax[i],address[i],phone_no[i]);
                suppliers_lis.Add(supplier_obj);

                context.suppliers_repository.Add(supplier_obj);
                context.SaveChanges();
            }

            // SUPPLIER WAREHOUSE
            // WAREHOUSE For FIRST SUPPLIER
            int[] quantity_item = new int[] { 50, 100, 75, 55, 125, 400, 500, 300, 100, 200, 800, 900, 700, 230, 840, 120, 320, 410, 470, 340, 320, 160, 750 };
            int[] price = new int[] { 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            int[] item_id = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };

            suppliers first_supplier_obj = new suppliers();
            List<suppliers_warehouse> suppliers_warehouse_lis = new List<suppliers_warehouse>();

            for(int k=0;k<3;k++)
            { 
                for (int i = 0; i < item_code.Length; i++)
                {
                    item item_obj = (item)item_list[i];
                    suppliers supplier_obj = (suppliers)suppliers_lis[k];

                    suppliers_warehouse supplier_ware_obj = new suppliers_warehouse(quantity_item[i], quantity_item[i], price[i]);

                    supplier_ware_obj.suppliers_warehouse_plus_item = item_obj;
                    supplier_ware_obj.suppliers_warehouse_plus_supplier = supplier_obj;

                    suppliers_warehouse_lis.Add(supplier_ware_obj);

                    context.suppliers_warehouse_repository.Add(supplier_ware_obj);
                    context.SaveChanges();
                }
            }

            // DEPARTMENT Initialization
            string[] depart_code = new string[] { "ENGL", "CPSC", "Stationery Store Inventory System", "REGR", "ZOOL" };
            string[] depart_name = new string[] { "English Dept", "Computer Science", "Commerce Dept", "Registrar Dept", "Zoology Dept" };
            string[] dept_contact_name = new string[] { "Mrs Pamela Kow", "Mr Wee Kian Fatt", "Mr Mohd. Azman", "Ms Helen Ho", "Mr. Peter Tan Ah Meng" };
            int[] dept_tel_no = new int[] { 8742234, 8901235, 8741284, 8901266, 8901266 };
            int[] dept_fax_no = new int[] { 8921456, 8921457, 8921256, 8921465, 8921465 };
            string[] dept_head_name = new string[] { "Prof Ezra Pound", "Dr. Soh Kian Wee", "Dr. Chia Leow Bee", "Mrs Low Kway Boo", "Prof Tan" };
            string[] collection_point_list = new string[] { "Stationery Store", "Managament School", "Medical School", "Engineering School", "Science School", "University Hospital" };
            string[] representative_name_list = new string[] { "KayKay", "KyawKyaw", "AungAung", "TinTin", "NawNaw" };
            List<department> depart_lis = new List<department>();

            for(int i=0;i<depart_code.Length;i++)
            {
                department dept_obj = new department(depart_code[i], depart_name[i], dept_contact_name[i], dept_tel_no[i], dept_fax_no[i], dept_head_name[i],collection_point_list[i],representative_name_list[i]);

                depart_lis.Add(dept_obj);

                context.department_repository.Add(dept_obj);
                context.SaveChanges();
            }

            // STAFF Initialization
            List<staff> staff_lis = new List<staff>();
            // EMPLOYEE INITIALIZATION
            // First Department 
            staff staff_obj = new staff("KayKay", "kay1999@gmail.com", "kay1234", 21, "male", "Employee", depart_lis[0]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("NayNay", "nay1999@gmail.com", "nay1234", 27, "female", "Employee", depart_lis[0]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("ZawZaw", "zaw1999@gmail.com", "zaw1234",32,"male", "Employee", depart_lis[0]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            // Second Department 
            staff_obj = new staff("KyawKyaw", "kyaw1999@gmail.com", "kyaw1234", 21, "male", "Employee", depart_lis[1]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("MgMg", "mg1999@gmail.com", "mg1234", 22, "male", "Employee", depart_lis[1]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            // Third Department 
            staff_obj = new staff("AungAung", "aung1999@gmail.com", "aung1234", 21, "male", "Employee", depart_lis[2]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("HtikeHtike", "htike1999@gmail.com", "htike1234", 22, "female", "Employee", depart_lis[2]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            // Fourth Department 
            staff_obj = new staff("TinTin", "tin1999@gmail.com", "tin1234", 24, "male", "Employee", depart_lis[3]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("MyintMyint", "myint1999@gmail.com", "myint1234", 22, "female", "Employee", depart_lis[3]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            // Fifth Department 
            staff_obj = new staff("NawNaw", "naw1999@gmail.com", "naw1234", 25, "male", "Employee", depart_lis[4]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("MawMaw", "maw1999@gmail.com", "maw1234", 29, "female", "Employee", depart_lis[4]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            // HEAD INITIALIZATION
            staff_obj = new staff("HtetMoeHlyan", "htet1999@gmail.com", "htet1234", 22, "male", "Head", depart_lis[0]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("KaungPyaeHtet", "kaung1999@gmail.com", "kaung1234", 24, "male", "Head", depart_lis[1]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("MyintThein", "thein1999@gmail.com", "myint1234", 22, "female", "Head", depart_lis[2]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("ChoChoMaw", "cho1999@gmail.com", "cho1234", 25, "female", "Head", depart_lis[3]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("MayThin", "may1999@gmail.com", "may1234", 29, "female", "Head", depart_lis[4]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            // SUPERVISOR INITIALIZATION
            staff_obj = new staff("YeeMonAung", "yee1999@gmail.com", "yee1234", 22, "female", "Supervisor", depart_lis[2]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            /*
            staff_obj = new staff("YanMueAung", "htet1999@gmail.com", "htet1234", 22, "male", "Supervisor", depart_lis[0]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("KaungKhantKyaw", "khant1999@gmail.com", "khant1234", 24, "male", "Supervisor", depart_lis[1]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("KhaingSuWai", "khaing1999@gmail.com", "khaing1234", 25, "female", "Supervisor", depart_lis[3]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            staff_obj = new staff("PhyuPhyuThein", "phyu1999@gmail.com", "phyu1234", 29, "female", "Supervisor", depart_lis[4]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();
            */
            // MANAGER INITIALIZATION
            staff_obj = new staff("NuNu", "nu1960@gmail.com", "nu1234", 61, "female", "Manager", depart_lis[2]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();

            // CLERK INITIALIZATION
            staff_obj = new staff("AyeMoePhyu", "amp1996@gmail.com", "amp1234", 29, "female", "Clerk", depart_lis[4]);
            staff_lis.Add(staff_obj);
            context.staff_repository.Add(staff_obj);
            context.SaveChanges();

            // STAFF REPRESENTATIVE
            
            // ORDER ITEM Initialization
            Random rand = new Random();
            int[] staff_member = new int[] { 0, 3, 5,9,10 };
            int[] specific_item_number = new int[] { 2, 5, 1, 4, 6, 8, 9, 13, 16, 20 };
            List<orders> order_item_lis = new List<orders>();

            for(int k=0;k<staff_member.Length;k++)
            {
                staff temp_staff = staff_lis[staff_member[k]];
                
                for (int j=0;j<specific_item_number.Length;j++)
                {
                    item temp_item = item_list[j];

                    orders temp_order_item_obj = new orders(temp_staff, temp_item, rand.Next(20, 100),DateTime.Now.ToString());

                    order_item_lis.Add(temp_order_item_obj);

                    context.orders_repository.Add(temp_order_item_obj);

                    context.SaveChanges();
                }
            }

            // ITEM WAREHOUSE Initialization
            int reorder_level = 5;
            int reorder_quantity = 10;
            string unit_of_measure = "each";

            using(var db = new DataBaseContext())
            {
                // ALL DATA
                List<suppliers_warehouse> sup_ware_lis = db.suppliers_warehouse_repository.Where(x=>x.suppliers_warehouse_plus_supplier.suppliersId==1).Select(x => x).ToList();
                for(int i=0;i< sup_ware_lis.Count;i++)
                {
                    //TARGET DATA
                    suppliers_warehouse sup_ware = sup_ware_lis[i];

                    //ITEM SPECIFIC
                    item specific_item_obj = db.item_repository.Where(x => x.itemId == sup_ware.suppliers_warehouse_plus_item.itemId).Select(x => x).FirstOrDefault();
                    int sup_balance = sup_ware.stock_balance;

                    if (sup_balance > reorder_level)
                    {
                        sup_balance = sup_balance - reorder_quantity;
                        items_warehouse item_ware = new items_warehouse(reorder_level, reorder_quantity, unit_of_measure, reorder_quantity, reorder_quantity, 0, 0, specific_item_obj);
                        
                        // SUPPLIER WAREHOUSE
                        suppliers_warehouse db_sup_ware = db.suppliers_warehouse_repository.Where(x => x.suppliers_warehouseId == sup_ware.suppliers_warehouseId).FirstOrDefault();
                        db_sup_ware.stock_balance = sup_balance;

                        // STOCK CARD INITIALIZATION
                        stock_card stock_card_obj = new stock_card(db_sup_ware.suppliers_warehouse_plus_supplier.name, DateTime.Now.ToString(), "+" + reorder_quantity, specific_item_obj);
                        db.stock_card_repository.Add(stock_card_obj);
                        db.SaveChanges();

                        db.item_warehouses_repository.Add(item_ware);
                        db.SaveChanges();
                        db.suppliers_warehouse_repository.Add(db_sup_ware);
                        db.SaveChanges();
                    }
                }
            }

            base.Seed(context);
        }
    }   
}