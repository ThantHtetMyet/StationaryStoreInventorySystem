using ssi_system_ver_i.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ssi_system_ver_i.Controllers
{
    public class ClerkController : Controller
    {

        // -------------------------------------------------------------------------------------
        // ------------------------------- DEPARTMENT REQUESTS -------------------------------------------
        public ActionResult View_Department_Request()
        {
            using(var db = new DataBaseContext())
            {
                var order_lis = db.orders_repository
                                .Join(
                                db.item_repository,
                                orders => orders.item_obj.itemId,
                                items => items.itemId,

                                (orders, items) => new
                                {
                                    order_obj = orders,
                                    item_obj = items
                                }
                    ).Join(
                        db.staff_repository,
                        orders => orders.order_obj.staff_obj.staffId,
                        staff => staff.staffId,

                        (orders, staff) => new
                        {
                            order_obj = orders.order_obj,
                            item_obj = orders.item_obj,
                            depart_obj = db.department_repository.Where(d => d.departmentId == staff.department_obj.departmentId).FirstOrDefault()
                        }
                 ).Where(x => x.order_obj.order_status == "Approved_by_Head").ToList();

                List<orders> temp_order_lis = new List<orders>();
                List<item> temp_item_lis = new List<item>();
                List<department> temp_depart_lis = new List<department>();

                foreach(var temp in order_lis)
                {
                    temp_order_lis.Add(temp.order_obj);
                    temp_item_lis.Add(temp.item_obj);
                    temp_depart_lis.Add(temp.depart_obj);
                }

                ViewBag.Order_List = temp_order_lis;
                ViewBag.Item_List = temp_item_lis;
                ViewBag.Department_List = temp_depart_lis;
            }
            return View();
        }

        // AJAX
        // Reject Department Requests
        public ActionResult Ajax_Reject_Department_Request(ajax_model ajax_model_data)
        {
            ajax_model ajax_data = new ajax_model
            {
                name = ajax_model_data.name,
                main_data = ajax_model_data.main_data,
            };

            using(var db = new DataBaseContext())
            {
                List<orders> order_lis = db.orders_repository.Where(or => or.staff_obj.department_obj.department_name == ajax_data.name).ToList();

                foreach(orders or in order_lis)
                {
                    or.order_status = "Rejected_by_Clerk";

                    db.SaveChanges();
                }
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        // Approve Department Request
        public ActionResult Ajax_Approve_Department_Request(ajax_model ajax_model_data)
        {
            string quantity_status = "";
            int stock_level = 0;
            string order_id_status = "";

            Dictionary<string, int> item_and_quantity_of_department = new Dictionary<string, int>();

            ajax_model ajax_data = new ajax_model
            {
                name = ajax_model_data.name,
                main_data = ajax_model_data.main_data,
            };

            using(var db = new DataBaseContext())
            {
                List<orders> order_lis = db.orders_repository.Where(or => or.staff_obj.department_obj.department_name == ajax_data.name && or.order_status=="Approved_by_Head").ToList();

                Dictionary<int, int> item_quantity = new Dictionary<int, int>();

                List<items_warehouse> item_ware_lis = db.item_warehouses_repository.ToList();

                foreach(items_warehouse temp_item in item_ware_lis)
                {
                    item_quantity.Add(temp_item.item.itemId, temp_item.stock_balance);
                }

                for (int i=0;i<order_lis.Count;i++)
                {
                    orders temp_order = order_lis[i];
                        
                    stock_level = item_quantity[temp_order.item_obj.itemId];

                    stock_level = stock_level - temp_order.proposed_quantity;

                    order_id_status = temp_order.ordersId.ToString();

                    // For Stock Card
                    item_and_quantity_of_department.Add(temp_order.item_obj.item_description, temp_order.proposed_quantity);

                    if (stock_level<0)
                    {
                        quantity_status = "OUT_OF_STOCK";
                        break;
                    }
                    else
                    {
                        item_quantity[temp_order.item_obj.itemId] = stock_level;
                    }
                }

                if(quantity_status != "OUT_OF_STOCK")
                {
                    foreach(KeyValuePair<int,int> data in item_quantity)
                    {
                        items_warehouse item_ware_obj = db.item_warehouses_repository.Where(k => k.item.itemId == data.Key).FirstOrDefault();
                        item_ware_obj.stock_balance = data.Value;
                        db.SaveChanges();
                        quantity_status = "QUANTITY_SUFFICIENT";
                        
                        // Add ACTUAL_QUANTITY and DELIVERY DATE
                        foreach (orders temp_order in order_lis)
                        {
                            temp_order.actual_delivered_quantity_by_clerk = temp_order.proposed_quantity;
                            temp_order.delivered_order_date = DateTime.Now.ToString();
                            db.SaveChanges();
                        }
                    }

                    // For Stock Card
                    foreach(KeyValuePair<string,int> temp_data in item_and_quantity_of_department)
                    {
                        item item_obj = db.item_repository.Where(i => i.item_description == temp_data.Key).FirstOrDefault();

                        // STOCK CARD UPDATE
                        stock_card stock_card_obj = new stock_card(ajax_data.name, DateTime.Now.ToString(), " - " + temp_data.Value, item_obj);
                        db.stock_card_repository.Add(stock_card_obj);
                        db.SaveChanges();
                    }

                    foreach(orders temp_order in order_lis)
                    {
                        temp_order.order_status = "Approved_by_Clerk";
                        db.SaveChanges();
                    }
                }
            }
            object reply_to_client = new
            {
                item_quantity_status = quantity_status,
                stock_level_status = stock_level,
                order_identity_status = order_id_status,
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        //---------------------------------------------------------------------------------------------
        // --------------------- ADJUSTMENT VOUCHER -----------------------------------------------------
        public ActionResult View_Adjustment_Voucher()
        {
            List<item> item_list = new List<item>();

            using (var db = new DataBaseContext())
            {
                List<adjustment_voucher> adjustment_lis = db.adjustment_voucher_repository.Select(x => x).ToList();

                List<int> test = new List<int>();
                foreach (var temp in adjustment_lis)
                {
                    test.Add(temp.item_obj.itemId);
                }

                ViewBag.adjustment_lis = (List<adjustment_voucher>)adjustment_lis;
                ViewBag.item_lis = test;
            }
                return View();
        }

        [HttpPost]
        public ActionResult Create_Adjustment_Voucher(FormCollection form_data)
        {
            string item_description = form_data["item_code_list"];
            string quantity_bx = form_data["quantity_bx"];
            string reason = form_data["adjustment_reason"];

            using(var db=new DataBaseContext())
            {
                item item_obj = db.item_repository.Where(x => x.item_description == item_description).FirstOrDefault();

                if(item_obj != null)
                {
                    adjustment_voucher adjustment_voucher_obj = new adjustment_voucher(item_obj,Int32.Parse(quantity_bx), reason,DateTime.Now.ToString(),"Added_by_Clerk");

                    db.adjustment_voucher_repository.Add(adjustment_voucher_obj);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("View_Adjustment_Voucher", "Clerk");
        }

        // ------------------------------------- AJAX -----------------------------------------------------
        [HttpPost]
        public JsonResult Ajax_Request_Item(ajax_model ajax_model_obj)
        {
            List<String> item_name_lis = new List<String>();

            ajax_model data = new ajax_model
            {
                name = ajax_model_obj.name,
                main_data = ajax_model_obj.main_data,
            };

            using(var db = new DataBaseContext())
            {
                List<item> all_item_lis = db.item_repository.ToList<item>();

                for (int i = 0; i < all_item_lis.Count; i++)
                {
                    item_name_lis.Add(all_item_lis[i].item_description);
                }


            }
            object reply_to_client = new
            {
               key_itemname_lis = item_name_lis,
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        //---------------------------------------------------------------------------------------------
        // --------------------------------------  PURCHASE ORDER -----------------------------------------------------
        public ActionResult View_Purchase_Order()
        {
            using(var db = new DataBaseContext())
            {
                List<items_warehouse> item_ware_lis = db.item_warehouses_repository.Where(i => i.stock_balance < i.reorder_level).ToList();
                for(int i=0;i<item_ware_lis.Count;i++)
                {
                    items_warehouse temp_item_ware_obj = item_ware_lis[i];
                    item_ware_lis[i].item = temp_item_ware_obj.item;
                }
                ViewBag.Item_WareHouse_lis = item_ware_lis;
            }
            return View();
        }

        [HttpPost]
        public JsonResult Ajax_Purchase_Order(ajax_model ajax_data)
        {
            ajax_model data = new ajax_model
            {
                name = ajax_data.name,
                main_data = ajax_data.main_data,
            };
            Dictionary<string, int> supplier_and_quantity;
            string supplier_status = "NOT_OUT_OF_STOCK_IN_SUPPLIER";

            using (var db = new DataBaseContext())
            {
                int temp_item_ware_id = Int32.Parse(data.name);

                items_warehouse item_ware_obj = db.item_warehouses_repository.Where(it => it.items_warehouseId == temp_item_ware_id).FirstOrDefault();

                supplier_and_quantity = SEND_ORDER_TO_SUPPLIERS(item_ware_obj);
                
                // Add Total Quantity From Suppliers
                foreach(KeyValuePair<string,int> temp_data in supplier_and_quantity)
                {
                    item_ware_obj.stock_balance = item_ware_obj.stock_balance + temp_data.Value;
                    suppliers sup_obj = db.suppliers_repository.Where(s => s.name == temp_data.Key).FirstOrDefault();
                    
                    if(temp_data.Key == "OUT_OF_STOCK_IN_SUPPLIER")
                    {
                        supplier_status = "OUT_OF_STOCK_IN_SUPPLIER";
                    }
                    else if(sup_obj.suppliersId == 1)
                    {
                        item_ware_obj.first_supplier_balance = item_ware_obj.first_supplier_balance + temp_data.Value;
                        db.SaveChanges();

                        // STOCK CARD UPDATE
                        stock_card stock_card_obj = new stock_card(sup_obj.name, DateTime.Now.ToString(), "+ " + temp_data.Value, item_ware_obj.item);
                        db.stock_card_repository.Add(stock_card_obj);
                        db.SaveChanges();
                    }
                    else if(sup_obj.suppliersId ==2)
                    {
                        item_ware_obj.second_supplier_balance = item_ware_obj.second_supplier_balance + temp_data.Value;
                        db.SaveChanges();

                        // STOCK CARD UPDATE
                        stock_card stock_card_obj = new stock_card(sup_obj.name, DateTime.Now.ToString(), "+ " + temp_data.Value, item_ware_obj.item);
                        db.stock_card_repository.Add(stock_card_obj);
                        db.SaveChanges();
                    }
                    else if(sup_obj.suppliersId == 3)
                    {
                        item_ware_obj.third_supplier_balance = item_ware_obj.third_supplier_balance + temp_data.Value;
                        db.SaveChanges();

                        // STOCK CARD UPDATE
                        stock_card stock_card_obj = new stock_card(sup_obj.name, DateTime.Now.ToString(), "+ " + temp_data.Value, item_ware_obj.item);
                        db.stock_card_repository.Add(stock_card_obj);
                        db.SaveChanges();
                    }
                }
            }
            object reply_to_client = new
            {
                supplier_and_quantity_key = supplier_and_quantity,
                supplier_out_of_stock_status = supplier_status,
            };
            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        public Dictionary<string,int> SEND_ORDER_TO_SUPPLIERS(items_warehouse item_ware_obj)
        {
            Dictionary<string, int> supplier_and_quantity = new Dictionary<string, int>();

            Boolean flag = false;
            Boolean supplier_flag = false;

            int target_quantity = item_ware_obj.reorder_quantity;

            int supplier_ID = 1;

            using (var db = new DataBaseContext())
            {
                do
                {
                    suppliers_warehouse target_sup_ware_obj = db.suppliers_warehouse_repository.Where(sup => sup.suppliers_warehouse_plus_supplier.suppliersId == supplier_ID && sup.suppliers_warehouse_plus_item.itemId==item_ware_obj.item.itemId).FirstOrDefault();
                    
                    // Check SUPPLIER_WAREHOUSE + ITEM_ID in ITEM_WAREHOUSE
                    if (target_sup_ware_obj.stock_balance > 0)
                    {

                    // SUFFICIENT
                    if (target_sup_ware_obj.stock_balance >= item_ware_obj.reorder_quantity)
                    {
                     target_sup_ware_obj.stock_balance = target_sup_ware_obj.stock_balance - target_quantity;
                     db.SaveChanges();
                     supplier_and_quantity.Add(target_sup_ware_obj.suppliers_warehouse_plus_supplier.name, target_quantity);
                      flag = true;
                      target_quantity = 0;
                      }
                       // NOT SUFFICIENT
                       else if(target_quantity!=0)
                       {
                        target_quantity = target_quantity - target_sup_ware_obj.stock_balance;
                         supplier_and_quantity.Add(target_sup_ware_obj.suppliers_warehouse_plus_supplier.name, target_sup_ware_obj.stock_balance);
                        target_sup_ware_obj.stock_balance = 0;
                        db.SaveChanges();
                        supplier_ID++;
                        }
                 }
                    else
                    {
                        supplier_ID++;
                    }

                if(supplier_ID==4)
                    {
                        supplier_flag = true;
                    }
                } while (!flag && !supplier_flag);

                if(supplier_flag)
                {
                    supplier_and_quantity.Add("OUT_OF_STOCK_IN_SUPPLIER", 404);
                }
            }
            return supplier_and_quantity;
        }

        //---------------------------------ChargeBack--------------------------------------//
        //---------------------------------------------------------------------------------//
        public ActionResult View_ChargeBack()
        {
            using (var db = new DataBaseContext())
            {

                var Result = db.orders_repository
                    .Join(
                        db.item_repository,

                        orders => orders.item_obj.itemId,
                        items => items.itemId,

                        (orders, items) => new
                        {
                            Order_obj = orders,
                            Item_obj = items

                        }
                    )
                    .Join(
                        db.staff_repository,
                        orders => orders.Order_obj.staff_obj.staffId,
                        staff => staff.staffId,
                        (orders, staff) => new
                        {

                            Order_Obj = orders.Order_obj,
                            Item_Obj = orders.Item_obj,
                            Staff_Obj = staff,
                            Dept_Obj = db.department_repository.Where(d => d.departmentId == staff.department_obj.departmentId).FirstOrDefault()
                        }
                 ).Where(x => x.Order_Obj.order_status == "Approved_by_Clerk").ToList();

                List<orders> temp_order_lis = new List<orders>();
                List<item> temp_item_lis = new List<item>();
                List<staff> temp_staff_lis = new List<staff>();
                List<department> temp_depart_lis = new List<department>();

                foreach (var temp in Result)
                {
                    temp_order_lis.Add(temp.Order_Obj);
                    temp_item_lis.Add(temp.Item_Obj);
                    temp_staff_lis.Add(temp.Staff_Obj);
                    temp_depart_lis.Add(temp.Dept_Obj);
                }

                ViewBag.Order_List = temp_order_lis;
                ViewBag.Item_List = temp_item_lis;
                ViewBag.Staff_List = temp_staff_lis;
                ViewBag.Department_List = temp_depart_lis;
            }
            return View();
        }

        //---------------------------------------------------------------------------------------
        // ------------------------------ STOCK CARD --------------------------------------------
        public ActionResult View_Stock_Card()
        {
            using(var db = new DataBaseContext())
            {
                List<stock_card> stock_card_lis = db.stock_card_repository.ToList();
                for(int i=0;i<stock_card_lis.Count;i++)
                {
                    stock_card temp_stock_card = stock_card_lis[i];

                    stock_card_lis[i].item_obj = temp_stock_card.item_obj;
                }

                ViewBag.Stock_Card_Lis = stock_card_lis;
            }

            return View();
        }

        //---------------------------------Stationary_Retrival_Form--------------------------------------//
        //--------------------------------------------------------------------------------------//
        public ActionResult View_Stationary_Retrival_Form()
        {
            using (var db = new DataBaseContext())
            {
                var Result = db.orders_repository.Join(
                    db.department_repository,

                    orders => orders.staff_obj.department_obj.departmentId,
                    department => department.departmentId,
                    (orders,department) => new
                    {
                        Order_Obj = orders,
                        Department_Obj = department
                    }

                    )
                    .Join(
                        db.item_warehouses_repository,

                        orders => orders.Order_Obj.item_obj.itemId,
                        items_warehouse => items_warehouse.item.itemId,

                        (orders, items_warehouse) => new
                        {
                            Department_Obj = orders.Department_Obj,
                            Order_Obj = orders,
                            Item_Ware_House_Obj = items_warehouse
                        }
                 ).Where(x => x.Order_Obj.Order_Obj.order_status == "Approved_by_Head").OrderBy(x => x.Order_Obj.Order_Obj.item_obj.itemId)
                 .ToList();  
                    
                List<orders> orders_lis = new List<orders>();
                List<items_warehouse> item_ware_lis = new List<items_warehouse>();
                List<department> depart_lis = new List<department>();
                
                for (int i=0;i<Result.Count;i++)
                {
                    var current_result = Result[i];
                    orders current_order_object = current_result.Order_Obj.Order_Obj;
                    items_warehouse item_ware_hourse = current_result.Item_Ware_House_Obj;
                    department department_obj = current_result.Department_Obj;

                    orders_lis.Add(current_order_object);
                    item_ware_lis.Add(item_ware_hourse);
                    depart_lis.Add(department_obj);
                }

                for(int i=0;i<orders_lis.Count;i++)
                {
                    orders_lis[i].item_obj = orders_lis[i].item_obj;
                    orders_lis[i].staff_obj = orders_lis[i].staff_obj;
                }

                ViewBag.Item_Warehouse_List = item_ware_lis;
                ViewBag.Order_List = orders_lis;
                ViewBag.Department_List = depart_lis;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create_Stationary_Retrival_Form(FormCollection form_data)
        {
            using (var db = new DataBaseContext())
            {
                int input_bx_count = Int32.Parse(form_data["input_bx_count_name"]);

                for (int i = 1; i <=input_bx_count; i++)
                {
                    string actual_quantity_delivered = form_data["Actual_Quantity_Delivered_" + i];
                    string store_order_id= form_data["Store_Order_ID_" + i];

                    string[] store_order_id_lis = store_order_id.Split('_');

                    System.Diagnostics.Debug.WriteLine("STORE ID LIST: " + store_order_id_lis);
                    System.Diagnostics.Debug.WriteLine("ACTUAL QUANTITY : " + actual_quantity_delivered);

                    if(store_order_id_lis.Length==1)
                    {
                        int order_id = Int32.Parse(store_order_id_lis[0]);
                        int actual_quantity = Int32.Parse(actual_quantity_delivered);
                        orders order_obj = db.orders_repository.Where(x => x.ordersId == order_id).FirstOrDefault();
                        order_obj.actual_delivered_quantity_by_clerk = actual_quantity;
                        order_obj.order_status = "Approved_by_Clerk";
                        db.SaveChanges();

                        items_warehouse item_ware_obj = db.item_warehouses_repository.Where(x => x.item.itemId == order_obj.item_obj.itemId).FirstOrDefault();
                        item_ware_obj.stock_balance = item_ware_obj.stock_balance - actual_quantity;
                        db.SaveChanges();
                    }
                    else
                    {
                        int actual_quantity = Int32.Parse(actual_quantity_delivered);
                        int temp_item_id = 0;

                        for (int j=0;j<store_order_id_lis.Length;j++)
                        {
                            int temp_order_id = Int32.Parse(store_order_id_lis[j]);
                            
                            orders order_obj = db.orders_repository.Where(x => x.ordersId == temp_order_id).FirstOrDefault();

                            temp_item_id = order_obj.item_obj.itemId;

                            if(actual_quantity - order_obj.proposed_quantity > 0 )
                            {
                                int temp_amount = actual_quantity - order_obj.proposed_quantity;
                                actual_quantity = actual_quantity - order_obj.proposed_quantity;

                                order_obj.actual_delivered_quantity_by_clerk = temp_amount;
                                order_obj.order_status = "Approved_by_Clerk";
                                db.SaveChanges();

                            }
                            else
                            {
                                order_obj.actual_delivered_quantity_by_clerk = actual_quantity;
                                order_obj.order_status = "Approved_by_Clerk";
                                db.SaveChanges();
                            }
                        }
                        items_warehouse item_ware_obj = db.item_warehouses_repository.Where(x => x.item.itemId == temp_item_id).FirstOrDefault();
                        item_ware_obj.stock_balance = item_ware_obj.stock_balance - actual_quantity;
                        db.SaveChanges();
                    }

                }
                return RedirectToAction("View_Adjustment_Voucher", "Clerk");
            }
        }
    }
}
