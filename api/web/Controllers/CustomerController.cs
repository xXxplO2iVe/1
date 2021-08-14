using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Web.Models;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public CustomerController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index()
        {
            var response = await Client.GetAsync("api/customer");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<List<Customer>>(result);
            return View(customer);
        }

        public async Task<IActionResult> LockUnlock(int id)
        {
            var response = await Client.PutAsync($"api/customer/{id}", null);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await Client.GetAsync($"api/customer/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(result);
            return View(customer);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = Client.PutAsync("api/customer", content).Result;

                if (response.IsSuccessStatusCode) 
                {
                return RedirectToAction("Index");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
            }

            return View(customer);
        }
    }
}
