using System.Linq;
using System.Collections.Generic;
using WebAPI.Data;
using WebAPI.Models.Repository;


namespace WebAPI.Models.DataManager
{
    public class AccountManager : IDataRepository<Account, int>
    {
        private readonly MCBAContext _context;
        public AccountManager(MCBAContext context)
        {
            _context = context;
        }

        public int Add(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account.AccountNumber;
        }

        public int Delete(int id)
        {
            _context.Accounts.Remove(_context.Accounts.Find(id));
            _context.SaveChanges();

            return id;
        }

        public Account Get(int id)
        {
            return _context.Accounts.Find(id);
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

        public int Update(int id, Account item)
        {
            throw new System.NotImplementedException();
        }
    }
}
