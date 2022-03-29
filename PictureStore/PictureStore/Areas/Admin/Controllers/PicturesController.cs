using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PictureStore.Models;

namespace PictureStore.Areas.Admin.Controllers
{
    public class PicturesController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();

        // GET: Admin/Pictures
        public ActionResult Index(string search, int? page, int? size)
        {
            var pictures = db.pictures.Include(p => p.category);
            if (!String.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                pictures = pictures.Where(p => p.name.Contains(search) || p.descript.Contains(search));
            }
            pictures = pictures.OrderBy(p => p.id);
            int pageNumber = (page ?? 1);
            int pageSize = (size ?? 5);
            ViewBag.size = pageSize;

            return View(pictures.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.pictures.Include(p => p.category).Where(p => p.id == id).First();
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Admin/Pictures/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.categories, "id", "name");
            var model = new Picture() { status = "Còn hàng", likeCount = 0 };
            return View(model);
        }

        // POST: Admin/Pictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,categoryId,price,descript,material,size,author,likeCount,imageUrl,status")] Picture picture, HttpPostedFileBase image)
        {
            if (image == null)
            {
                ModelState.AddModelError("imageUrl", "Ảnh không được trống");
            }

            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/images/pictures/"), Path.GetFileName(image.FileName));
                image.SaveAs(path);
                picture.imageUrl = ("/images/pictures/" + image.FileName);

                db.pictures.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoryId = new SelectList(db.categories, "id", "name", picture.categoryId);
            return View(picture);
        }

        // GET: Admin/Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryId = new SelectList(db.categories, "id", "name", picture.categoryId);
            return View(picture);
        }

        // POST: Admin/Pictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,categoryId,price,descript,material,size,author,likeCount,imageUrl,status")] Picture picture, HttpPostedFileBase image, string imageOld)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string path = Path.Combine(Server.MapPath("~/images/pictures/"), Path.GetFileName(image.FileName));
                    image.SaveAs(path);
                    picture.imageUrl = ("/images/pictures/" + image.FileName);
                } 
                else
                {
                    picture.imageUrl = imageOld;
                }
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.categories, "id", "name", picture.categoryId);
            return View(picture);
        }

        // GET: Admin/Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.pictures.Include(p => p.category).Where(p => p.id == id).First();
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Admin/Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.pictures.Find(id);
            db.pictures.Remove(picture);
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
