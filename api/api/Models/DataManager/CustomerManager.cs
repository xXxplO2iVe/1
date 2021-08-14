using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models.Repository;
using System.Linq;


namespace WebAPI.Models.DataManager
{
    public class CustomerManager : IDataRepository<Customer, int>
    {
        private readonly MCBAContext _context;
        public CustomerManager(MCBAContext context)
        {
            _context = context;
        }
        public int Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Customers.Remove(_context.Customers.Find(id));
            _context.SaveChanges();

            return id;
        }

        public Customer Get(int id)
        {
            return _context.Customers.Find(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }
        public int Update(int id, Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

            return id;
        }

        public void LockUnlock(int id)
        {
            var customer = Get(id);

            if (customer.Locked == true)
            {
                customer.Locked = false;
            }
            else
            {
                customer.Locked = true;
            }

            _context.SaveChanges();
        }
    }
}
