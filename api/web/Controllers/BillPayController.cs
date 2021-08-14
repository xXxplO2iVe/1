using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Web.Models;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class BillPayController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public BillPayController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index()
        {
            var response = await Client.GetAsync($"api/billpay");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = response.Content.ReadAsStringAsync().Result;
            var billPay = JsonConvert.DeserializeObject<List<BillPay>>(result);
            return View(billPay);
        }

        public async Task<IActionResult> BlockUnblock(int id)
        {
            var response = await Client.PutAsync($"api/billpay/{id}", null);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return RedirectToAction("Index");
        }
    }
}
