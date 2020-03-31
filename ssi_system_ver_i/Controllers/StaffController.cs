using ssi_system_ver_i.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ssi_system_ver_i.Controllers
{
    public class StaffController : Controller
    {
        public ActionResult Log_In()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(FormCollection sign_in_data)
        {
            string email_val = sign_in_data["log_email_bx"];
            string password_val = sign_in_data["login_passwd_bx"];

            using (var db = new DataBaseContext())
            {

                staff staff_obj = db.staff_repository.Where(x => x.email == email_val).FirstOrDefault<staff>();
                
                if(staff_obj==null)
                {
                    return RedirectToAction("SignIn", "Staff");
                }

                else
                {
                    if (staff_obj.password.Equals(password_val))
                    {
                        
                        staff_representative staff_repre_obj = db.staff_representative_repository.Where(s_re => s_re.representative_staff_obj.staffId == staff_obj.staffId).FirstOrDefault();

                        if(staff_repre_obj!=null)
                        {
                            if(staff_repre_obj.position == "Temporary_Head")
                            {


                                Session["UserName"] = staff_obj.name;
                                Session["Role"] = "Temporary_Head";
                                return RedirectToAction("View_Staff_Request", "Head");
                            }
                        }

                        else if (staff_obj.position.Equals("Employee"))
                        {
                            Session["UserName"] = staff_obj.name;
                            Session["Role"] = "Employee";
                            return RedirectToAction("View_Request_Item", "Staff");
                        }

                        else if (staff_obj.position.Equals("Head"))
                        {
                            Session["UserName"] = staff_obj.name;
                            Session["Role"] = "Head";
                            return RedirectToAction("View_Staff_Request", "Head");
                        }

                        else if (staff_obj.position.Equals("Clerk"))
                        {
                            Session["UserName"] = staff_obj.name;
                            Session["Role"] = "Clerk";

                            return RedirectToAction("View_Department_Request", "Clerk");
                        }

                        else if (staff_obj.position.Equals("Manager"))
                        {
                            Session["UserName"] = staff_obj.name;
                            Session["Role"] = "Manager";
                            return RedirectToAction("View_Manager_Adjustment_Voucher", "Manager");
                        }

                        else if (staff_obj.position.Equals("Supervisor"))
                        {
                            Session["UserName"] = staff_obj.name;
                            Session["Role"] = "Supervisor";
                            return RedirectToAction("View_Supervisor_Adjustment_Voucher", "Supervisor");
                        }

                        else if (staff_obj.position.Equals("Representative"))
                        {
                            Session["UserName"] = staff_obj.name;
                            Session["Role"] = "Representative";
                            return RedirectToAction("View_Collect_Order", "Representative");
                        }
                    }

                    else
                    {
                        return RedirectToAction("Log_In", "Staff");
                    }
                }
                
            }
            return RedirectToAction("Log_In", "Staff");
        }

        // ------------------------------------------------------------------------------------
        // --------------------------  CREATE REQUEST ITEM ------------------------------------------
        public ActionResult View_Request_Item()
        {
            string user_name = Session["UserName"].ToString();

            using(var db = new DataBaseContext())
            {
                staff staff_obj = db.staff_repository.Where(x => x.name == user_name).Select(x => x).FirstOrDefault();

                List<orders> orders_lis = db.orders_repository.Where(x => x.staff_obj.staffId == staff_obj.staffId && x.order_status=="pending").ToList();

                List<string> item_description_lis = new List<string>();
                foreach(orders or in orders_lis)
                {
                    item_description_lis.Add(or.item_obj.item_description);
                }

                ViewBag.item_description_lis = item_description_lis;
                ViewBag.order_lis = (List<orders>)orders_lis;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create_New_Request_Item(FormCollection form_data)
        {
            int num_of_item_request = Int32.Parse(form_data["number_of_request_items"]);

             if(num_of_item_request == 3)
            {
                string item_description = form_data["item_code_list_3"];
                string quantity_bx = form_data["quantity_bx_3"];
                string user_name = Session["UserName"].ToString();

                using (var db = new DataBaseContext())
                {
                    staff staff_obj = db.staff_repository.Where(x => x.name == user_name).Select(x => x).FirstOrDefault();

                        item item_obj = db.item_repository.Where(x => x.item_description == item_description).FirstOrDefault();

                        orders order_obj = new orders(staff_obj, item_obj, Int32.Parse(quantity_bx), DateTime.Now.ToString());

                        db.orders_repository.Add(order_obj);
                        db.SaveChanges();
                }
            }
             else
            {
                List<String> item_code_lis = new List<String>();
                List<String> quantity_lis = new List<String>();

                for (int j = 3; j <= num_of_item_request; j++)
                {
                    string item_description = form_data["item_code_list_" + j];
                    string quantity_bx = form_data["quantity_bx_" + j];

                    item_code_lis.Add(item_description);
                    quantity_lis.Add(quantity_bx);
                }

                string user_name = Session["UserName"].ToString();

                using (var db = new DataBaseContext())
                {
                    staff staff_obj = db.staff_repository.Where(x => x.name == user_name).Select(x => x).FirstOrDefault();

                    for (int j = 0; j < item_code_lis.Count; j++)
                    {
                        string item_description = item_code_lis[j];
                        string quantity_bx = quantity_lis[j];

                        item item_obj = db.item_repository.Where(x => x.item_description == item_description).FirstOrDefault();

                        orders order_obj = new orders(staff_obj, item_obj, Int32.Parse(quantity_bx), DateTime.Now.ToString());

                        db.orders_repository.Add(order_obj);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("View_Request_Item", "Staff");
        }

        // -----------------------------------------------------------------------------------//
        // ------------------------------------  LOG OUT -------------------------------------//
        public ActionResult Log_Out()
        {
            Session.Clear();
            return RedirectToAction("Log_In", "Staff");
        }

        // -----------------------------------------------------------------------------------//
        // ------------------------------------  EDIT REQUEST ITEM -------------------------------------//
        [HttpPost]
        public ActionResult Edit_New_Request_Item(FormCollection form_data)
        {
            string order_id = form_data["edit_order_id"];
            string item_code = form_data["edit_item_code"]; 
            string quantity_bx = form_data["edit_quantity_bx"];

            string user_name = Session["UserName"].ToString();

            using (var db = new DataBaseContext())
            {
                int target_id = Int32.Parse(order_id);
                int target_quantity = Int32.Parse(quantity_bx);

                orders order_obj = db.orders_repository.Where(x => x.ordersId == target_id).FirstOrDefault();
                order_obj.proposed_quantity = target_quantity;

                db.SaveChanges();
            }

            return RedirectToAction("View_Request_Item", "Staff");
        }

        // -----------------------------------------------------------------------------------------------
        // ------------------------------------- AJAX -----------------------------------------------------
        [HttpPost]
        public JsonResult Ajax_Add_Request_Item(ajax_model ajax_model_obj)
        {
            List<String> item_name_lis = new List<String>();

            ajax_model data = new ajax_model
            {
                name = ajax_model_obj.name,
                main_data = ajax_model_obj.main_data,
            };

            using (var db = new DataBaseContext())
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

        [HttpPost]
        public JsonResult Ajax_Delete_Request_Item(ajax_model ajax_model_obj)
        {
            ajax_model data = new ajax_model
            {
                name = ajax_model_obj.name,
                main_data = ajax_model_obj.main_data,
            };

            using (var db = new DataBaseContext())
            {
                int target_order_item = Int32.Parse(data.name);

                orders order_item = db.orders_repository.Where(x => x.ordersId == target_order_item).FirstOrDefault();

                db.orders_repository.Remove(order_item);
                db.SaveChanges();

            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Ajax_Edit_Request_Item(ajax_model ajax_model_obj)
        {
            orders order_item;

            ajax_model data = new ajax_model
            {
                name = ajax_model_obj.name,
                main_data = ajax_model_obj.main_data,
            };

            using (var db = new DataBaseContext())
            {
                int target_order_item = Int32.Parse(data.name);

                order_item = db.orders_repository.Where(x => x.ordersId == target_order_item).FirstOrDefault();


                object reply_to_client = new
                {
                    order_id = order_item.ordersId,
                    order_item_description = order_item.item_obj.item_description,
                    order_quantity = order_item.proposed_quantity
                };
                return Json(reply_to_client, JsonRequestBehavior.AllowGet);
            }
            
            //return Json(null, JsonRequestBehavior.AllowGet);
        }


        // -----------------------------------------------------------------------------------//
        // ------------------------------------  History_Request_Items-------------------------------------//

        public ActionResult View_History_Request_Items()
        {
            using(var db = new DataBaseContext())
            {
                string user_name = Session["UserName"].ToString();

                staff current_staff_obj = db.staff_repository.Where(x => x.name == user_name).Select(x => x).FirstOrDefault();

                List<orders> orders_lis = db.orders_repository.Where(or => or.order_status == "Approved_by_Clerk" && or.staff_obj.staffId == current_staff_obj.staffId).ToList();

                for(int i=0;i<orders_lis.Count;i++)
                {
                    orders temp_order = orders_lis[i];
                    orders_lis[i].item_obj = temp_order.item_obj;

                }
                ViewBag.order_lis = (List<orders>)orders_lis;
            }

                return View();
        }

    }


}