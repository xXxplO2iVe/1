using System.Collections.Generic;
using WebAPI.Models.DataManager;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace WebAPI.Models.DataController
{
    [ApiController, Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

        // GET: api/admin
        [HttpGet]
        public string Get()
        {
            return "Admin Portal";
        }
    }
}