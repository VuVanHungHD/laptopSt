using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PictureStore.Models;

namespace PictureStore.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();

        // GET: Admin/Users
        public ActionResult Index(string search, int? page, int? size)
        {
            var users = db.users.Select(c => c);
            if (!String.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                users = users.Where(c => c.username.Contains(search) || c.phonenumber.Contains(search) || c.email.Contains(search));
            }
            users = users.OrderBy(c => c.id);
            int pageNumber = (page ?? 1);
            int pageSize = (size ?? 5);
            ViewBag.size = pageSize;

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Categories/Details
        public ActionResult Details()
        {
            var id = Session["USER"];
            
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session["USER"] = null;
            Session["USER_NAME"] = null;
            return Redirect("/Users/Login");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,username,email,phonenumber,address,password,userType,status")] User user, string oldPassword, string newPassword, string repeatNewPassword)
        {
            var users = db.users.AsNoTracking().Where(u => u.email == user.email).ToList();
            if (users.Count() != 0 && users[0].id != user.id)
            {
                ModelState.AddModelError("email", "Email này đã được đăng kí!");
                return RedirectToAction("Details", user);
            }

            var userInDB = db.users.AsNoTracking().Where(u => u.id == user.id).First();
            if (oldPassword == "" && newPassword == "" && repeatNewPassword == "")
            {
                //không đổi pass
                user.password = userInDB.password;
            }
            else
            {
                //đổi pass
                //check old pass

                if (verifyPassword(oldPassword, userInDB.password))
                {
                    if (newPassword == repeatNewPassword)
                    {
                        //đổi pass
                        user.password = enscriptPassword(newPassword);
                    }
                    else
                    {
                        ModelState.AddModelError("password", "Mật khẩu mới hoặc nhập lại mật khẩu mới sai!");
                    }
                }
                else
                {
                    ModelState.AddModelError("password", "Mật khẩu cũ sai!");
                    return RedirectToAction("Details", user);
                }

            }
            user.status = "Đang hoạt động";
            user.userType = "USER";
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", user);
            }
            return RedirectToAction("Details", user);
        }

        [HttpPost]
        public ActionResult ResetPassword(int id)
        {
            var user = db.users.Find(id);
            user.password = enscriptPassword(user.email);
            db.SaveChanges();
            return Json(new { status = "success" });
        }

        [HttpPost]
        public ActionResult Lock(int id)
        {
            var user = db.users.Find(id);
            user.status = "Bị khóa";
            db.SaveChanges();
            return Json(new { status = "success" });
        }

        [HttpPost]
        public ActionResult UnLock(int id)
        {
            var user = db.users.Find(id);
            user.status = "Đang hoạt động";
            db.SaveChanges();
            return Json(new { status = "success" });
        }

        private string enscriptPassword(string password)
        {
            //link tham khảo: https://stackoverflow.com/questions/4181198/how-to-hash-a-password
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            return savedPasswordHash;
        }

        private bool verifyPassword(string password, string savedPasswordHash)
        {
            //link tham khảo: https://stackoverflow.com/questions/4181198/how-to-hash-a-password

            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
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
