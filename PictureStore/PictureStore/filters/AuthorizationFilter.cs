using PictureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PictureStore.filters
{
    public class AuthorizationFilter: IActionFilter
    {
        private PictureStoreContext db = new PictureStoreContext();
        private Dictionary<string, string> mapUrlAndRoles = new Dictionary<string, string>()
        {
            { "/Admin/", "ADMIN" },
            { "/Users/Details", "USER, ADMIN" },
            { "/Bill", "USER, ADMIN" },
        };

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url.ToString();

            var pairs = mapUrlAndRoles.Where(m => url.Contains(m.Key)).ToList();
            if(pairs.Count != 0)
            {
                var pair = pairs[0];
                var roles = pair.Value;

                var id = filterContext.HttpContext.Session["USER"];
                if (id == null)
                {
                    filterContext.Result = new RedirectResult("/Users/Login");
                }
                else
                {
                    var user = db.users.Find(id);
                    if(!roles.Contains(user.userType))
                    {
                        filterContext.Result = new RedirectResult("/Users/Login");
                    }
                }
            }
            
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

    }
}