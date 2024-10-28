using BTTH_MVC_DotNet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BTTH_MVC_DotNet.Controllers
{
    public class AuthenController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(Account account)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                HttpContext.Session.SetString("UserName", account.UserName);

                if (account.UserName == "admin")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Products");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Authen");
        }


        public IActionResult ConfirmLogin()
        {
            string userName = Request.Form["UserName"];
            string password = Request.Form["Password"];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
            };

            // Tạo claims identity
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Đăng nhập bằng cookie
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Giữ trạng thái đăng nhập ngay cả khi đóng trình duyệt
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30) // Thời gian hết hạn cookie
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties).Wait();

            return RedirectToAction("Index", "Products");
        }
    }
}
