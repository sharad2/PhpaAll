//using PhpaBills.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;

namespace PhpaAll.Bills
{
    internal class ManageBillsRepository : IDisposable
    {
        private readonly PhpaBillsDataContext _db;

        public ManageBillsRepository(string connectString)
        {
            _db = new PhpaBillsDataContext(connectString);
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }

        public void InsertBill(Bill bill)
        {
            _db.Bills.InsertOnSubmit(bill);

            _db.SubmitChanges();


        }

        public IQueryable<Bill> Bills
        {
            get
            {
                return _db.Bills;
            }
        }



        public Bill GetBillNumber(string id)
        {
            return (from bill in _db.Bills
                    where bill.BillNumber == id
                    select bill).FirstOrDefault();

         
        }
    }
}