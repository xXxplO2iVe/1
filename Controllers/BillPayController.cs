using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Assignment_2.Data;
using Assignment_2.Models;

namespace Assignment_2.Controllers
{
    public class BillPayController : Controller
    {
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value; 
        private readonly MCBAContext _context;

        public BillPayController(MCBAContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.FindAsync(CustomerID));
        }

        public IActionResult CreateBill() => View();
       
        [HttpPost]
        public async Task<IActionResult> CreateBill(int id, BillPay billPay)
        {
            var account = await _context.Accounts.FindAsync(id);
            var payee = await _context.Payees.FindAsync(billPay.PayeeID);

            if (payee == null)
            {
                ModelState.AddModelError(nameof(billPay.PayeeID), "The payee ID does not exist.");
                ViewBag.Amount = billPay.Amount;
                return View(billPay);
            }

            if (ModelState.IsValid)
            {
                billPay.AccountNumber = id;
                billPay.ScheduleTimeUtc = billPay.ScheduleTimeUtc.ToUniversalTime();
                account.BillPay(billPay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(billPay);
        }

        public async Task<IActionResult> ViewBill(int id) => View(await _context.Accounts.FindAsync(id));

        public async Task<IActionResult> EditBill(int id)
        {
            var billPay = await _context.BillPays.FindAsync(id);
            return View(billPay);
        }

        [HttpPost]
        public async Task<IActionResult> EditBill(int id, BillPay billPay)
        {
            var account = await _context.Accounts.FindAsync(billPay.AccountNumber);
            var payee = await _context.Payees.FindAsync(billPay.PayeeID);

            if (payee == null)
            {
                ModelState.AddModelError(nameof(billPay.PayeeID), "The payee ID does not exist.");
                ViewBag.Amount = billPay.Amount;
                return View(billPay);
            }

            if (ModelState.IsValid)
            {
                var bill = await _context.BillPays.FindAsync(id);
                billPay.ScheduleTimeUtc = billPay.ScheduleTimeUtc.ToUniversalTime();
                bill.Update(billPay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(billPay);
        }

        public async Task<IActionResult> DeleteBill(int id)
        {
            var billPay = await _context.BillPays.FindAsync(id);
            _context.BillPays.Remove(billPay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
