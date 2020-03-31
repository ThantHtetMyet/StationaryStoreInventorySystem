using ssi_system_ver_i.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ssi_system_ver_i.Controllers
{
    public class HeadController : Controller
    {

        public ActionResult View_Staff_Request()
        {
            using (var db = new DataBaseContext())
            {
                String username = Session["UserName"].ToString();

                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x=>x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<orders> order_lis = db.orders_repository.Where(o => o.staff_obj.department_obj.department_name == department_name && o.order_status=="pending").Select(x=>x).ToList();

                for(int j=0;j<order_lis.Count;j++)
                {
                    orders or = order_lis[j];

                    staff temp_staff = db.staff_repository.Where(s => s.staffId == or.staff_obj.staffId).FirstOrDefault();
                    item temp_item = db.item_repository.Where(i => i.itemId == or.item_obj.itemId).FirstOrDefault();

                    order_lis[j].staff_obj = temp_staff;
                    order_lis[j].item_obj = temp_item;
                }
                ViewBag.Dept_name = department_name;
                ViewBag.Order_lis = (List<orders>)order_lis;
            }
            return View();
        }
        
        public ActionResult View_Approved_Staff_Request()
        {
            using (var db = new DataBaseContext())
            {
                String username = Session["UserName"].ToString();

                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<orders> order_lis = db.orders_repository.Where(o => o.staff_obj.department_obj.department_name == department_name && o.order_status == "Approved_by_Head").Select(x => x).ToList();

                for (int j = 0; j < order_lis.Count; j++)
                {
                    orders or = order_lis[j];

                    staff temp_staff = db.staff_repository.Where(s => s.staffId == or.staff_obj.staffId).FirstOrDefault();
                    item temp_item = db.item_repository.Where(i => i.itemId == or.item_obj.itemId).FirstOrDefault();

                    order_lis[j].staff_obj = temp_staff;
                    order_lis[j].item_obj = temp_item;
                }
                ViewBag.Dept_name = department_name;
                ViewBag.Order_lis = (List<orders>)order_lis;
            }
            return View();
        }

        public ActionResult View_Reject_Staff_Request()
        {
            using (var db = new DataBaseContext())
            {
                String username = Session["UserName"].ToString();

                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<orders> order_lis = db.orders_repository.Where(o => o.staff_obj.department_obj.department_name == department_name && o.order_status == "Reject_by_Head").Select(x => x).ToList();

                for (int j = 0; j < order_lis.Count; j++)
                {
                    orders or = order_lis[j];

                    staff temp_staff = db.staff_repository.Where(s => s.staffId == or.staff_obj.staffId).FirstOrDefault();
                    item temp_item = db.item_repository.Where(i => i.itemId == or.item_obj.itemId).FirstOrDefault();

                    order_lis[j].staff_obj = temp_staff;
                    order_lis[j].item_obj = temp_item;
                }
                ViewBag.Dept_name = department_name;
                ViewBag.Order_lis = (List<orders>)order_lis;
            }
            return View();
        }

        //----------------------------------------------------------------------------------------
        // ---------------------------- DELEGATE AUTHORITY ---------------------------------------
        public ActionResult Delegate_Authority()
        {
            using (var db = new DataBaseContext())
            {
                String username = Session["UserName"].ToString();

                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<staff> staff_lis = db.staff_repository.Where(s => s.department_obj.department_name == department_name && (s.position=="Employee" || s.position=="Representative") ).ToList();

                ViewBag.Dept_name = department_name;
                ViewBag.Staff_lis = staff_lis;

                foreach(staff temp_staff in staff_lis)
                {
                    staff_representative staff_representative_obj = db.staff_representative_repository.Where(s_re => s_re.representative_staff_obj.staffId == temp_staff.staffId && s_re.position == "Temporary_Head").FirstOrDefault();

                    if(staff_representative_obj != null)
                    {
                        ViewBag.Staff_Representative = staff_representative_obj;
                    }
                }
            }
                return View();
        }


        // ----------------------------------------------------------------------------------------------
        // *****************************   AJAX ********************************************************
        [HttpPost]
        public ActionResult Ajax_Approve_Request_Item(ajax_model ajax_data)
        {
            ajax_model data = new ajax_model
            {
                name = ajax_data.name,
                main_data = ajax_data.main_data,
            };

            using(var db = new DataBaseContext())
            {
                int temp_id = Int32.Parse(data.main_data[0]);

                orders temp_order = db.orders_repository.Where(o => o.ordersId == temp_id).FirstOrDefault();
                temp_order.order_status = "Approved_by_Head";

                db.SaveChanges();
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Ajax_Reject_Request_Item(ajax_model ajax_data)
        {
            ajax_model data = new ajax_model
            {
                name = ajax_data.name,
                main_data = ajax_data.main_data,
            };

            using (var db = new DataBaseContext())
            {
                int temp_id = Int32.Parse(data.main_data[0]);

                orders temp_order = db.orders_repository.Where(o => o.ordersId == temp_id).FirstOrDefault();
                temp_order.order_status = "Reject_by_Head";

                db.SaveChanges();
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ajax_Cancel_Approve_Request_Item(ajax_model ajax_data)
        {
            ajax_model data = new ajax_model
            {
                name = ajax_data.name,
                main_data = ajax_data.main_data,
            };

            using (var db = new DataBaseContext())
            {
                int temp_id = Int32.Parse(data.main_data[0]);

                orders temp_order = db.orders_repository.Where(o => o.ordersId == temp_id).FirstOrDefault();
                temp_order.order_status = "Pending";

                db.SaveChanges();
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ajax_Cancel_Reject_Request_Item(ajax_model ajax_data)
        {
            ajax_model data = new ajax_model
            {
                name = ajax_data.name,
                main_data = ajax_data.main_data,
            };

            using (var db = new DataBaseContext())
            {
                int temp_id = Int32.Parse(data.main_data[0]);

                orders temp_order = db.orders_repository.Where(o => o.ordersId == temp_id).FirstOrDefault();
                temp_order.order_status = "Pending";

                db.SaveChanges();
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        // ------------------------------------------------------------------------------------------------
        // -------------------  DELEGATE AUTHORITY -----------------------------------------------------------

        [HttpPost]
        public ActionResult Delegate_Authority_Staff(FormCollection form_data)
        {
            string staff_name = form_data["staff_list"];
            string duty_start_date = form_data["start_date_box"].ToString();
            string duty_end_date = form_data["end_date_box"].ToString();

            using (var db = new DataBaseContext())
            {
                staff staff_obj = db.staff_repository.Where(s => s.name == staff_name).FirstOrDefault();

                staff_representative staff_representative_obj = new staff_representative(duty_start_date, duty_end_date, staff_obj, "Temporary_Head");

                db.staff_representative_repository.Add(staff_representative_obj);
                db.SaveChanges();

            }
            return RedirectToAction("Delegate_Authority", "Head");
        }

        // Cancel Delegate Authority
        [HttpPost]
        public ActionResult Ajax_Cancel_Authority(ajax_model ajax_data)
        {
            ajax_model data = new ajax_model
            {
                name = ajax_data.name,
                main_data = ajax_data.main_data,
            };

            using (var db = new DataBaseContext())
            {
                int temp_id = Int32.Parse(data.name);

                staff_representative staff_repre_obj = db.staff_representative_repository.Where(s => s.staff_representativeId == temp_id).FirstOrDefault();

                // Change position "Representative" to "Employee"
                //staff staff = db.staff_repository.Where(s => s.staffId == staff_repre_obj.representative_staff_obj.staffId).FirstOrDefault();
                //staff.position = "Employee";

                db.staff_representative_repository.Remove(staff_repre_obj);

                db.SaveChanges();
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Ajax_Delegate_Autority_Request_Item(ajax_model ajax_model_data)
        {
            using (var db = new DataBaseContext())
            {
                String username = Session["UserName"].ToString();

                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<staff> staff_lis = db.staff_repository.Where(s => s.department_obj.department_name == department_name && s.position == "Employee").ToList();

                List<String> staff_name_lis = new List<String>();

                foreach (staff temp_staff in staff_lis)
                {
                    staff_name_lis.Add(temp_staff.name);
                }

                object reply_to_client = new
                {
                    key_staff_lis = staff_name_lis,
                };
                return Json(reply_to_client, JsonRequestBehavior.AllowGet);
            }
        }

        // ------------------------------------------------------------------------------------------------
        // ------------------- REPRESENTATIVE -----------------------------------------------------------
        public ActionResult Representative_Selection()
        {
            using (var db = new DataBaseContext())
            {
                String username = Session["UserName"].ToString();
                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();
                string department_name = staff_obj.department_obj.department_name;
                List<staff> staff_lis = db.staff_repository.Where(s => s.department_obj.department_name == department_name && s.position !="Head" && s.position!="Supervisor").ToList();
                ViewBag.Dept_name = department_name;
                ViewBag.Staff_lis = staff_lis;

                foreach (staff temp_staff in staff_lis)
                {
                    staff_representative staff_representative_obj = db.staff_representative_repository.Where(s_re => s_re.representative_staff_obj.staffId == temp_staff.staffId).FirstOrDefault();

                    if (staff_representative_obj != null)
                    {
                        ViewBag.Staff_Representative = staff_representative_obj;
                    }
                }
            }
            return View();
        }

        public ActionResult Ajax_Representative_Autority_Request_Item(ajax_model ajax_model_data)
        {
            using (var db = new DataBaseContext())
            {
                String username = Session["UserName"].ToString();

                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<staff> staff_lis = db.staff_repository.Where(s => s.department_obj.department_name == department_name && s.position == "Employee").ToList();

                List<String> staff_name_lis = new List<String>();

                foreach (staff temp_staff in staff_lis)
                {
                    staff_name_lis.Add(temp_staff.name);
                }

                object reply_to_client = new
                {
                    key_staff_lis = staff_name_lis,
                };
                return Json(reply_to_client, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Representative_Authority_Staff(FormCollection form_model_data)
        {
            string staff_name = form_model_data["staff_list"];
           
            using (var db = new DataBaseContext())
            {
                // Check Already Representative Staff
                List<staff> representative_staff_obj = db.staff_repository.Where(s => s.position == "Representative").ToList();
                if(representative_staff_obj != null)
                {
                    foreach(staff temp in representative_staff_obj)
                    {
                        temp.position = "Employee";
                        db.SaveChanges();
                    }
                }

                // Create Representative Staff
                staff staff_obj = db.staff_repository.Where(s => s.name == staff_name).FirstOrDefault();
                staff_obj.position = "Representative";
                db.SaveChanges();

                // Change Representative_Name In Department Table
                String username = Session["UserName"].ToString();
                staff second_staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();
                string department_name = staff_obj.department_obj.department_name;

                department depart_obj = db.department_repository.Where(d => d.department_name == department_name).FirstOrDefault();
                depart_obj.representative_name = staff_name;

                db.SaveChanges();
            }
            return RedirectToAction("Representative_Selection", "Head");
        }

        // Cancel Representative Staff
        public ActionResult Ajax_Cancel_Representative_Staff(ajax_model ajax_data)
        {
            ajax_model data = new ajax_model
            {
                name = ajax_data.name,
                main_data = ajax_data.main_data,
            };

            using (var db = new DataBaseContext())
            {
                int temp_id = Int32.Parse(data.name);

                staff_representative staff_repre_obj = db.staff_representative_repository.Where(s => s.staff_representativeId == temp_id).FirstOrDefault();

                // Change position "Representative" to "Employee"
                //staff staff = db.staff_repository.Where(s => s.staffId == staff_repre_obj.representative_staff_obj.staffId).FirstOrDefault();
                //staff.position = "Employee";

                db.staff_representative_repository.Remove(staff_repre_obj);

                db.SaveChanges();
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };
            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        // ------------------------------------------------------------------------------------------------
        // ------------------- COLLECTION POINT -----------------------------------------------------------
        public ActionResult Collection_Point_Change()
        {
            using (var db = new DataBaseContext())
            {
                
                String username = Session["UserName"].ToString();

                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                department depart_obj = db.department_repository.Where(d => d.department_name == department_name).FirstOrDefault();

                ViewBag.Dept_name = department_name;
                ViewBag.Department_Obj = depart_obj;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Ajax_Change_Collection_Point(ajax_model ajax_data)
        {
            string[] collection_point_list = new string[] { "Stationery Store", "Managament School", "Medical School", "Engineering School", "Science School", "University Hospital" };

            object reply_to_client = new
            {
                key_staff_lis = collection_point_list,
            };
            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Change_Collection_Point(FormCollection form_data)
        {
            string collection_point_name = form_data["collection_point_list"];

            using(var db = new DataBaseContext())
            {
                String username = Session["UserName"].ToString();
                staff staff_obj = db.staff_repository.Where(s => s.name == username).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                department depart_obj = db.department_repository.Where(d => d.department_name == department_name).FirstOrDefault();
                depart_obj.collection_point = collection_point_name;

                db.SaveChanges();
            }
            return RedirectToAction("Collection_Point_Change", "Head");
        }
    }
}