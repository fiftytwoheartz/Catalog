using System.Web.Mvc;

namespace Catalog.WebSite.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}