using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PictureStore.Models;
using PagedList;

namespace PictureStore.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();

        // GET: Admin/Categories
        public ActionResult Index(string search, int? page, int? size)
        {
            var categories = db.categories.Select(c => c);
            if(!String.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                categories = categories.Where(c => c.name.Contains(search) || c.descript.Contains(search));
            }
            categories = categories.OrderBy(c => c.id);
            int pageNumber = (page ?? 1);
            int pageSize = (size ?? 5);
            ViewBag.size = pageSize;

            return View(categories.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            var categories = db.categories.ToList();
            Category emptyCategory = new Category();
            categories.Insert(0, emptyCategory);
            SelectList cateList = new SelectList(categories, "id", "name");
            ViewBag.CategoryList = cateList;
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,descript,parentId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var categories = db.categories.Where(c => c.id != id).ToList();
            Category emptyCategory = new Category();
            categories.Insert(0, emptyCategory);
            SelectList cateList = new SelectList(categories, "id", "name");
            ViewBag.CategoryList = cateList;
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,descript,parentId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.categories.Find(id);
            db.categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
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
