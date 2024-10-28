using Microsoft.AspNetCore.Mvc;

namespace BTTH_MVC_DotNet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();      
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Authen", new { area = "" });
        }

    }
}
