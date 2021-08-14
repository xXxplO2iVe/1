using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models.Repository;


namespace WebAPI.Models.DataManager
{
    public class BillPayManager : IDataRepository<BillPay, int>
    {
        private readonly MCBAContext _context;
        public BillPayManager(MCBAContext context)
        {
            _context = context;
        }

        public int Add(BillPay billPay)
        {
            _context.BillPays.Add(billPay);
            _context.SaveChanges();

            return billPay.BillPayID;
        }

        public int Delete(int id)
        {
            _context.BillPays.Remove(_context.BillPays.Find(id));
            _context.SaveChanges();

            return id;
        }

        public BillPay Get(int id)
        {
            return _context.BillPays.Find(id);
        }

        public IEnumerable<BillPay> GetAll()
        {
            return _context.BillPays.ToList();
        }

        public int Update(int id, BillPay item)
        {
            throw new System.NotImplementedException();
        }

        public void BlockUnblock(int id)
        {
            var billPay = Get(id);

            if (billPay.Blocked == true)
            {
                billPay.Blocked = false;
            }
            else
            {
                billPay.Blocked = true;
            }

             _context.SaveChanges();
        }
    }
}
