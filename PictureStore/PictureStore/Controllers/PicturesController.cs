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

namespace PictureStore.Controllers
{
    public class PicturesController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();

        // GET: Pictures
        public ActionResult Index(string search, int? page, int? size, string orderBy, string orderType)
        {
            var pictures = db.pictures.Include(p => p.category);
            if (!String.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                pictures = pictures.Where(p => p.name.Contains(search) || p.descript.Contains(search));
            }

            if(!String.IsNullOrEmpty(orderType) && orderType == "DESC")
            {
                ViewBag.orderType = "DESC";
                switch (orderBy)
                {
                    case "id":
                        pictures = pictures.OrderByDescending(p => p.id);
                        ViewBag.orderBy = "id";
                        break;
                    case "name":
                        pictures = pictures.OrderByDescending(p => p.name);
                        ViewBag.orderBy = "name";
                        break;
                    case "price":
                        pictures = pictures.OrderByDescending(p => p.price);
                        ViewBag.orderBy = "price";
                        break;
                    case "likeCount":
                        pictures = pictures.OrderByDescending(p => p.likeCount);
                        ViewBag.orderBy = "likeCount";
                        break;
                    default:
                        pictures = pictures.OrderByDescending(p => p.id);
                        ViewBag.orderBy = "id";
                        break;
                }
            }
            else
            {
                ViewBag.orderType = "ASC";
                switch (orderBy)
                {
                    case "id":
                        pictures = pictures.OrderBy(p => p.id);
                        ViewBag.orderBy = "id";
                        break;
                    case "name":
                        pictures = pictures.OrderBy(p => p.name);
                        ViewBag.orderBy = "name";
                        break;
                    case "price":
                        pictures = pictures.OrderBy(p => p.price);
                        ViewBag.orderBy = "price";
                        break;
                    case "likeCount":
                        pictures = pictures.OrderBy(p => p.likeCount);
                        ViewBag.orderBy = "likeCount";
                        break;
                    default:
                        pictures = pictures.OrderBy(p => p.id);
                        ViewBag.orderBy = "id";
                        break;
                }
            }    

            int pageNumber = (page ?? 1);
            int pageSize = (size ?? 12);
            ViewBag.size = pageSize;

            return View(pictures.ToPagedList(pageNumber, pageSize));
        }

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.pictures.Include(p => p.category).Where(p => p.id == id).FirstOrDefault();
            if (picture == null)
            {
                return HttpNotFound();
            }

            ViewBag.relatedPictures = db.pictures.Where(p => p.status == "Còn hàng" && p.category.id == picture.category.id).Take(8).ToList();
            return View(picture);
        }

        // GET: Pictures/Details/5
        public ActionResult Like(int id)
        {
            var pic = db.pictures.Find(id);
            pic.likeCount = pic.likeCount + 1;
            db.SaveChanges();

            return Json(new { status = "success" });
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
