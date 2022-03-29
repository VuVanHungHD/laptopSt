using System.Web;
using System.Web.Mvc;
using PictureStore.filters;

namespace PictureStore
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizationFilter());
            filters.Add(new MenuFilter());
        }
    }
}
