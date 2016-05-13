using System.Web;
using System.Web.Mvc;

namespace Statium.Alpha.Remote
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
