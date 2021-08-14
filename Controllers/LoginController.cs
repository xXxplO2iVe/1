using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment_2.Data;
using Assignment_2.Models;
using Microsoft.AspNetCore.Http;
using SimpleHashing;

namespace Assignment_2.Controllers
{
    public class LoginController : Controller
    {
        private readonly MCBAContext _context;

        public LoginController(MCBAContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(string loginID, string password)
        {
            var login = await _context.Logins.FindAsync(loginID);

            if (login.Customer.Locked==true)
            {
                ModelState.AddModelError("LoginFailed", "Login failed, Account is locked by Admin.");
                return View(new Login { LoginID = loginID });
            }

            if (login == null || !PBKDF2.Verify(login.PasswordHash, password))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new Login { LoginID = loginID });
            }

            // Login customer.
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Login.LoginID), login.LoginID);
            HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name); 

            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
