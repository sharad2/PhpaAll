//using PhpaBills.Database;
using System;
using System.Configuration;
using System.Linq;

namespace PhpaAll.Bills
{
    internal class ManageBillsService:IDisposable
    {
        private readonly ManageBillsRepository _repos;

        public ManageBillsService(string connectStringName)
        {
            var connectString = ConfigurationManager.ConnectionStrings[connectStringName].ConnectionString;
            _repos = new ManageBillsRepository(connectString);
        }

        public void Dispose()
        {
            if (_repos != null)
            {
                _repos.Dispose();
            }
        }

        public void InsertBill(Bill bill)
        {
            _repos.InsertBill(bill);
        }

        public IQueryable<Bill> Bills
        {
            get
            {
                return _repos.Bills;
            }
        }



        public Bill GetBillNumber(int id)
        {
            return _repos.GetBillNumber(id);
        }

        public void UpdateBill(Bill billmodel)
        {
            _repos.UpdateBill(billmodel);
        }

        public void DeleteBill(int id)
        {
            _repos.DeleteBill(id);
        }
    }
}