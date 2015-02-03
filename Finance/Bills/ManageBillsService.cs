//using PhpaBills.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Web;

namespace PhpaAll.Bills
{
    [Obsolete]
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

        public IQueryable<BillAudit> BillAudit
        {
            get
            {
                return _db.BillAudits;
            }
        }
       

        public void UpdateBill(Bill bill)
        {
            _db.SubmitChanges();
        }

        public void DeleteBill(int id)
        {
            Bill edit = _db.Bills.Where(bill => bill.Id == id).SingleOrDefault();
            _db.Bills.DeleteOnSubmit(edit);
            _db.SubmitChanges();
        }

        /// <summary>
        /// To get division list for division Auto Complete text box
        /// </summary>
        /// <param name="searchText">
        /// Search term is passed to populate the list
        /// </param>
        /// <returns></returns>        
        public IList<Tuple<string, string>> GetDivisions(string searchId, string searchDescription)
        {
            throw new NotImplementedException();

        }
    }
}