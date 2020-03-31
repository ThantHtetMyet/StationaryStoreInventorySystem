using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ssi_system_ver_i.Models;

namespace ssi_system_ver_i.Controllers
{
    public class AndroidController : Controller
    {
        // GET: Android
        public ActionResult Index()
        {
            return View();
        }


        //login method
        [HttpPost]
        public JsonResult Login(staff staff)
        {
            string email_val = staff.email;
            string password_val = staff.password;
            if (staff != null)
            {
                using (var db = new DataBaseContext())
                {

                    staff staff_obj = db.staff_repository.Where(x => x.email == email_val).FirstOrDefault<staff>();

                    if (staff_obj == null)
                    {
                        return Json(new { status = "fail" });
                    }

                    else
                    {
                        if (staff_obj.password.Equals(password_val))
                        {

                            /*Session["UserName"] = staff_obj.name;
                            Session["Role"] = staff_obj.position;*/
                            string name = staff_obj.name;
                            string email = staff_obj.email;
                            string password = staff_obj.password;
                            string role = staff_obj.position;


                            return Json(new { status = "ok", name = name, email = email, password = password, position = role });




                        }
                        else
                            return Json(new { status = "fail" });

                    }

                }

            }
            else
                return Json(new { status = "fail" });
        }

        //retrive list of employee
        [HttpPost]
        public ActionResult Delegate_Authority(string name)
        {
            using (var db = new DataBaseContext())
            {

                staff staff_obj = db.staff_repository.Where(s => s.name == name).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<staff> staff_lis = db.staff_repository.Where(s => s.department_obj.department_name == department_name && (s.position == "Employee" || s.position == "Representative")).ToList();

                return Json(new { status = "ok", staff_lis });
            }

        }
        //delegate new employee
        [HttpPost]
        public ActionResult Delegate_Authority_Staff(string staff_name, string duty_start_date, string duty_end_date)
        {




            using (var db = new DataBaseContext())
            {
                staff staff_obj = db.staff_repository.Where(s => s.name == staff_name).FirstOrDefault();

                staff_representative staff_representative_obj = new staff_representative(duty_start_date, duty_end_date, staff_obj, "Temporary_Head");

                db.staff_representative_repository.Add(staff_representative_obj);
                db.SaveChanges();

            }

            return Json(new { status = "ok" });
        }
        //retrieve list of delegated authorities
        [HttpPost]
        public ActionResult Cancel_Delegate_Authority(string name)
        {
            using (var db = new DataBaseContext())
            {


                staff staff_obj = db.staff_repository.Where(s => s.name == name).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<staff> staff_lis = db.staff_repository.Where(s => s.department_obj.department_name == department_name && (s.position == "Employee" || s.position == "Representative")).ToList();




                List<staff_representative> staff_representative_obj = db.staff_representative_repository.Where(s_re => s_re.representative_staff_obj.department_obj.department_name == department_name).ToList();


                return Json(new { status = "ok", staff_representative_obj });
            }

        }
        //Cancel delegate authority
        [HttpPost]
        public ActionResult Cancel_Delegate_Item(int temp_id)
        {
            using (var db = new DataBaseContext())
            {
                //int temp_id = Int32.Parse(data.name);

                staff_representative staff_repre_obj = db.staff_representative_repository.Where(s => s.staff_representativeId == temp_id).FirstOrDefault();

                // Change position "Representative" to "Employee"
                //staff staff = db.staff_repository.Where(s => s.staffId == staff_repre_obj.representative_staff_obj.staffId).FirstOrDefault();
                //staff.position = "Employee";

                db.staff_representative_repository.Remove(staff_repre_obj);

                db.SaveChanges();
            }
            return Json(new { status = "ok", });
        }

        //retrive list of orders request for approval for dept manager
        public ActionResult View_Staff_Request(String name)
        {
            using (var db = new DataBaseContext())
            {
                

                staff staff_obj = db.staff_repository.Where(s => s.name == name).Select(x => x).FirstOrDefault();

                string department_name = staff_obj.department_obj.department_name;

                List<orders> order_lis = db.orders_repository.Where(o => o.staff_obj.department_obj.department_name == department_name && o.order_status == "pending").Select(x => x).ToList();

                for (int j = 0; j < order_lis.Count; j++)
                {
                    orders or = order_lis[j];

                    staff temp_staff = db.staff_repository.Where(s => s.staffId == or.staff_obj.staffId).FirstOrDefault();
                    item temp_item = db.item_repository.Where(i => i.itemId == or.item_obj.itemId).FirstOrDefault();

                    order_lis[j].staff_obj = temp_staff;
                    order_lis[j].item_obj = temp_item;
                }
    

                return Json(new { status = "ok", department_name, order_lis });
            }
            

        }
        [HttpPost]
        public ActionResult Ajax_Approve_Request_Item(int temp_id)
        {
          

            using (var db = new DataBaseContext())
            {
                

                orders temp_order = db.orders_repository.Where(o => o.ordersId == temp_id).FirstOrDefault();
                temp_order.order_status = "Approved_by_Head";

                db.SaveChanges();
            }


            return Json(new { status = "ok" });
        }

    }

}
   

