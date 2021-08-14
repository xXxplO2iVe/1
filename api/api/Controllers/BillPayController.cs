using System.Collections.Generic;
using WebAPI.Models.DataManager;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace WebAPI.Models.DataController
{
    [ApiController, Route("api/[controller]")]
    public class BillPayController : ControllerBase
    {
        private readonly BillPayManager _repo;
        public BillPayController(BillPayManager repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        public BillPay Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpGet]
        public IEnumerable<BillPay> GetAll()
        {
            return _repo.GetAll();
        }

        [HttpPut("{id}")]
        public void BlockUnblock(int id)
        {
           _repo.BlockUnblock(id);
        }
    }
}