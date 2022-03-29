using PictureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PictureStore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.pictureLikeMost = db.pictures.OrderByDescending(p => p.likeCount).First();
            try
            {
                ViewBag.moneyInMoth = db.bills.Where(b => b.dateCreate.Month == DateTime.Now.Month && b.status == "Đã giao").Sum(b => b.total);
                ViewBag.totalMoney = db.bills.Where(b => b.status == "Đã giao").Sum(b => b.total);
            }catch(Exception ex)
            {
                ViewBag.moneyInMoth = 0;
                ViewBag.totalMoney = 0;
            }
            int[] dataOfYear = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i =0; i<12; i++)
            {
                int month = i + 1;
                int data = 0;
                try
                {
                   data = db.bills.Where(b => b.dateCreate.Month == month && b.status == "Đã giao").Sum(b => b.total);
                } catch(Exception ex)
                {

                }
                dataOfYear[i] = data;
            }
            ViewBag.dataOfYear = dataOfYear;
            return View();
        }
    }
}
