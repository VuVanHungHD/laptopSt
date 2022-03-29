using PagedList;
using PictureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PictureStore.Controllers
{
    public class BillController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();
        // GET: Bill
        public ActionResult Index(int? page, int? size)
        {
            var userId = (int)Session["USER"]; 
            var bills = db.bills.Where(b => b.userId == userId);
            bills = bills.OrderBy(p => p.id);
            int pageNumber = (page ?? 1);
            int pageSize = (size ?? 5);
            ViewBag.size = pageSize;

            return View(bills.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddBill()
        {
            ViewBag.user = db.users.Find(Session["USER"]);
            var cart = new Dictionary<Picture, int>();
            var total = 0;
            if (Session["CART"] != null)
            {
                foreach (var c in (Dictionary<int, int>)Session["CART"])
                {
                    var picture = db.pictures.Where(p => p.id == c.Key).First();
                    cart.Add(picture, c.Value);
                    total += picture.price * c.Value;
                }
            }
            ViewBag.cart = cart;
            ViewBag.total = total;
            return View();
        }

        public ActionResult SaveBill()
        {
            var bill = new Bill();
            bill.userId = (int) Session["USER"];
            var total = 0;
            foreach (var c in (Dictionary<int, int>)Session["CART"])
            {
                var picture = db.pictures.Where(p => p.id == c.Key).First();
                db.billDetails.Add(new BillDetail()
                {
                    picture = picture,
                    bill = bill,
                    count = c.Value
                });
                total += picture.price * c.Value;
            }
            bill.total = total;
            bill.dateCreate = DateTime.Now;
            bill.status = "Đang giao";
            db.SaveChanges();

            Session["CART"] = null;

            return Redirect("/Bill/Index");
        }

        public ActionResult Details(int id)
        {
            ViewBag.bill = db.bills.Find(id);
            var billDetails = new Dictionary<Picture, int>();
            var total = 0;
            var details = db.billDetails.Where(bdt => bdt.billId == id).ToList();
            foreach(var dt in details)
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
            return Redirect("/Bill/Index");
        }

        public ActionResult Delete(int id)
        {
            var bill = db.bills.Find(id);
            bill.status = "Đã hủy";
            db.SaveChanges();
            return Redirect("/Bill/Index");
        }
    }
}
