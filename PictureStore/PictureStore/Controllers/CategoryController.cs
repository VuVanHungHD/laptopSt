using PictureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PictureStore.Controllers
{
    public class CategoryController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();

        // GET: Category
        public ActionResult Index()
        {
            ViewBag.categories = db.categories.ToList();
            return View();
        }

        public ActionResult Details(int id)
        {
            ViewBag.category = db.categories.Find(id);
            ViewBag.categoriesChild = db.categories.Where(c => c.parentId == id).ToList();
            ViewBag.pictures = db.pictures.Where(p => p.categoryId == id).ToList();
            return View();
        }
    }
}