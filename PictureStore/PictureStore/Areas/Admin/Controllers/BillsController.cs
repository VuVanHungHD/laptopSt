using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PictureStore.Models;

namespace PictureStore.Areas.Admin.Controllers
{
    public class BillsController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();

        // GET: Admin/Bills
        public ActionResult Index(int? page, int? size)
        {
            var bills = db.bills.Include(b => b.user);
            bills = bills.OrderBy(p => p.id);
            int pageNumber = (page ?? 1);
            int pageSize = (size ?? 5);
            ViewBag.size = pageSize;

            return View(bills.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id)
        {
            ViewBag.bill = db.bills.Find(id);
            var billDetails = new Dictionary<Picture, int>();
            var total = 0;
            var details = db.billDetails.Where(bdt => bdt.billId == id).ToList();
            foreach (var dt in details)
            {
                var pictureId = dt.pictureId;
                var picture = db.pictures.Find(pictureId);
                billDetails.Add(picture, dt.count);
                total += picture.price * dt.count;
            }
            ViewBag.billDetails = billDetails;
            ViewBag.total = total;
            return View();
        }
        public ActionResult Done(int id)
        {
            var bill = db.bills.Find(id);
            bill.status = "Đã giao";
            db.SaveChanges();
            return Redirect("/Admin/Bills/Index");
        }


        public ActionResult Delete(int id)
        {
            var bill = db.bills.Find(id);
            bill.status = "Đã hủy";
            db.SaveChanges();
            return Redirect("/Admin/Bills/Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
