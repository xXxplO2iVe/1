using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (username.Equals("admin") && password.Equals("admin"))
            {
                // Login admin.
                HttpContext.Session.SetString("Username", "admin");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
            return View();
        }

        public IActionResult Logout()
        {
            // Logout admin.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
