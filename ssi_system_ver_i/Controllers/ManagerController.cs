using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ssi_system_ver_i.Models;

namespace ssi_system_ver_i.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        // ------------------------------- View Adjustment_Voucher -------------------------------------------
        public ActionResult View_Manager_Adjustment_Voucher()
        {
            using (var db = new DataBaseContext())
            {

                var adj_gt = db.adjustment_voucher_repository.Join(
                    db.item_repository,
                    adjust => adjust.item_obj.itemId,
                    item => item.itemId,
                    (adjust, item) => new
                    {
                        adjust_obj = adjust,
                        item_obj = item

                    }
                    ).Where(x => x.adjust_obj.status == "Added_by_Clerk").ToList();

                List<adjustment_voucher> temp_adj_lis = new List<adjustment_voucher>();
                List<item> temp_item_lis = new List<item>();

                foreach (var temp in adj_gt)
                {
                    temp_adj_lis.Add(temp.adjust_obj);
                    temp_item_lis.Add(temp.item_obj);
                }

                ViewBag.Adjust_List = temp_adj_lis;
                ViewBag.Item_List = temp_item_lis;

            }
            return View();
        }
        // ------------------------------- View_Maintain_SupplierList -------------------------------------------
        public ActionResult View_Maintain_SupplierList()
        {
            using (var db = new DataBaseContext())
            {

                var suppliers = db.suppliers_repository.Select(x => x).ToList();
                List<suppliers> temp_sup_lis = new List<suppliers>();
                foreach (var temp in suppliers)
                {
                    temp_sup_lis.Add(temp);

                }

                ViewBag.Sup_List = temp_sup_lis;
            }
            return View();
        }

        public JsonResult Ajax_Approve_Adjustment_ID(ajax_model ajax_data)
        {
            using (var db = new DataBaseContext())
            {
                int adjust_id = Int32.Parse(ajax_data.name);

                adjustment_voucher adjust_obj = db.adjustment_voucher_repository.Where(x => x.adjustment_voucherId == adjust_id).FirstOrDefault();
                adjust_obj.status = "Approved_by_Manager";

                db.SaveChanges();
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Ajax_Reject_Adjustment_ID(ajax_model ajax_data)
        {
            using (var db = new DataBaseContext())
            {
                int adjust_id = Int32.Parse(ajax_data.name);

                adjustment_voucher adjust_obj = db.adjustment_voucher_repository.Where(x => x.adjustment_voucherId == adjust_id).FirstOrDefault();
                adjust_obj.status = "Rejected_by_Manager";

                db.SaveChanges();
            }
            object reply_to_client = new
            {
                key_itemname_lis = "SUCCESS",
            };

            return Json(reply_to_client, JsonRequestBehavior.AllowGet);
        }
    }
}