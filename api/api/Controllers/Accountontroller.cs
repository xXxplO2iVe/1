using WebAPI.Models.DataManager;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace WebAPI.Models.DataController
{
    [ApiController, Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountManager _repo;
        public AccountController(AccountManager repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        public Account GetAccount(int id)
        {
            return _repo.Get(id);
        }

        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _repo.GetAll();
        }
    }
}
