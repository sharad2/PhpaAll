//using PhpaBills.Database;
using System;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Web;

namespace PhpaAll.Bills
{
    internal class ManageBillsService:IDisposable
    {
        private readonly PhpaBillsDataContext _db;

        private readonly StringWriter _sw;

        private readonly TraceContext _trace;

        public ManageBillsService(string connectStringName, TraceContext trace)
        {
            var connectString = ConfigurationManager.ConnectionStrings[connectStringName].ConnectionString;
            _db = new PhpaBillsDataContext(connectString);
            _sw = new StringWriter();
            _db.Log = _sw;
            _trace = trace;

            //var dlo = new DataLoadOptions();
            //dlo.LoadWith<Bill>(p => p.Contractor);
            //dlo.LoadWith<Bill>(p => p.Division);
            //_db.LoadOptions = dlo;
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            if (_sw != null)
            {
                _trace.Write(_sw.ToString());
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


        [Obsolete]
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