using PictureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PictureStore.Controllers
{
    public class HomeController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();
        public ActionResult Index()
        {
            
            ViewBag.top6Like = db.pictures.Where(p => p.status == "Còn hàng").OrderByDescending(p => p.likeCount).Take(6);
            ViewBag.top8New = db.pictures.Where(p => p.status == "Còn hàng").OrderByDescending(p => p.createDate).Take(8);
            ViewBag.top6InCate1 = db.pictures.Where(p => p.status == "Còn hàng" && p.categoryId == 1).OrderByDescending(p => p.id).Take(6).ToList();
            ViewBag.top6InCate2 = db.pictures.Where(p => p.status == "Còn hàng" && p.categoryId == 2).OrderByDescending(p => p.id).Take(6).ToList();
            ViewBag.top6InCate3 = db.pictures.Where(p => p.status == "Còn hàng" && p.categoryId == 1003).OrderByDescending(p => p.id).Take(6).ToList();
            return View();
        }
        public ActionResult test()
        {
            return View();
        }

    }
}
