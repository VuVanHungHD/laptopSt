using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using PictureStore.Models;

namespace PictureStore.Controllers
{
    public class UsersController : Controller
    {
        private PictureStoreContext db = new PictureStoreContext();

        // GET: Users
        public ActionResult Index()
        {
            return Redirect("/Users/Details");
        }

        // GET: Users/Details
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

        // GET: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "id,username,email,phonenumber,address,password,userType,status")] User user, string repeatPassword)
        {
            if (db.users.Where(u => u.email == user.email).Count() != 0)
            {
                ModelState.AddModelError("email", "Email đăng kí đã tồn tại!");
            }

            if(!repeatPassword.Equals(user.password))
            {
                ModelState.AddModelError("password", "Mật khẩu và mật khẩu được nhập lại không giống nhau!");
            }

            if (ModelState.IsValid)
            {
                user.password = enscriptPassword(user.password);
                user.status = "Đang hoạt động";
                user.userType = "USER";
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["USER"] = null;
            Session["USER_NAME"] = null;
            return Redirect("/Users/Login");
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var users = db.users.Where(u => u.email == email).ToList();
            if (users.Count() == 0)
            {
                ModelState.AddModelError("email", "Tài khoản ứng với email này không tồn tại!");
                return View();
            }
            else
            {
                var user = users[0];
                if(verifyPassword(password, user.password))
                {
                    if(user.status != "Đang hoạt động")
                    {
                        ModelState.AddModelError("email", "Tài khoản này đã bị khóa!");
                        return View();
                    }
                    Session["USER"] = user.id;
                    Session["USER_NAME"] = user.username;
                    if (user.userType == "ADMIN")
                    {
                        return Redirect("/Admin/Home/Index");
                    } 
                    else
                    {
                        return Redirect("/Home/Index");
                    }    
                }
                else
                {
                    
                    ModelState.AddModelError("password", "Mật khẩu sai!");
                    return View();
                }    
            }
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,username,email,phonenumber,address,password,userType,status")] User user, string oldPassword, string newPassword, string repeatNewPassword)
        {
            var users = db.users.AsNoTracking().Where(u => u.email == user.email).ToList();
            if (users.Count() !=0 && users[0].id != user.id)
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
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", user);
            }
            return RedirectToAction("Details", user);
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
