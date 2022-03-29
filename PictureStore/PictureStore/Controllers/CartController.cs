using Newtonsoft.Json;
using PictureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PictureStore.Controllers
{
    public class CartController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();
        public ActionResult Index()
        {
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

        [HttpPost]
        public ActionResult AddToCart(int idPicture, int? countPicture)
        {

            if (Session["CART"] == null)
            {
                var cart = new Dictionary<int, int>();//id sp - so luong
                int count = (countPicture ?? 1);
                cart.Add(idPicture, count);
                Session["CART"] = cart;
            }
            else
            {
                var cart = (Dictionary<int, int>)Session["CART"];
                if (cart.ContainsKey(idPicture))
                {
                    if (countPicture == null)
                    {
                        cart[idPicture] = cart[idPicture]++;
                    }
                    else
                    {
                        cart[idPicture] = (int) countPicture;
                    }    
                }
                else
                {
                    int count = (countPicture ?? 1);
                    cart.Add(idPicture, count);
                }
            }

            return Json(new { status = "success" });
        }

        [HttpPost]
        public ActionResult DeleteItemInCart(int idPicture)
        {
            var cart = (Dictionary<int, int>)Session["CART"];
            cart.Remove(idPicture);
            return Redirect("/Cart");
        }

        public ActionResult UpdateCart(string jsonCart)
        {
            var cart = (Dictionary<int, int>)Session["CART"];
            var cartUpdate = JsonConvert.DeserializeObject<Dictionary<int, int>>(jsonCart);
            if (cartUpdate != null)
            {
                foreach (var c in cartUpdate)
                {
                    cart[c.Key] = c.Value;
                }
            }

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