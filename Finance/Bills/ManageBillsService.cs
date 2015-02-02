//using PhpaBills.Database;
using System;
using System.Configuration;
using System.Linq;

namespace PhpaAll.Bills
{
    internal class ManageBillsService:IDisposable
    {
        private readonly PhpaBillsDataContext _db;

        public ManageBillsService(string connectStringName)
        {
            var connectString = ConfigurationManager.ConnectionStrings[connectStringName].ConnectionString;
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



        public Bill GetBillNumber(int id)
        {
            return (from bill in _db.Bills
                    where bill.Id == id
                    select bill).FirstOrDefault();
        }

        public void UpdateBill(Bill bill)
        {
            var edit = (from b in _db.Bills
                        where b.Id == bill.Id
                        select b).SingleOrDefault();

            edit.Amount = bill.Amount;
            edit.Particulars = bill.Particulars;
            edit.BillNumber = bill.BillNumber;
            edit.BillDate = bill.BillDate;
            edit.BillImage = bill.BillImage;
            edit.ContractorId = bill.ContractorId;
            edit.SubmitedToDivisionId = bill.SubmitedToDivisionId;
            edit.DueDate = bill.DueDate;
            edit.PaidDate = bill.PaidDate;
            edit.Remarks = bill.Remarks;
            edit.SubmittedOnDate = bill.SubmittedOnDate;

            _db.SubmitChanges();
        }

        public void DeleteBill(int id)
        {
            Bill edit = _db.Bills.Where(bill => bill.Id == id).SingleOrDefault();
            _db.Bills.DeleteOnSubmit(edit);
            _db.SubmitChanges();
        }
    }
}