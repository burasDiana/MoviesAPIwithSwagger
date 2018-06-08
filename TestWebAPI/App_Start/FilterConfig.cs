using System.Web;
using System.Web.Mvc;
using TestWebAPI.Security;

namespace TestWebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
