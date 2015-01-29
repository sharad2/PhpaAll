﻿//using PhpaBills.Database;
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


        //Gettting the details of passed BillNumber to be updated
        public Bill GetBillNumber(int id)
        {
            return (from bill in _db.Bills
                    where bill.Id == id
                    select bill).FirstOrDefault();


        }


        //Updating the BillNumber
        public void UpdateBill(CreateViewModel model)
        {
            Bill edit = _db.Bills.Where(bill => bill.Id == model.Id).SingleOrDefault();

            edit.Amount = model.Amount;
            edit.ApprovedBy = model.ApprovedBy;
            edit.BillNumber = model.BillNumber;
            edit.ApprovedOn = model.ApprovedDate;
            edit.BillDate = model.BillDate;
            edit.BillType = model.BillType;
            edit.ContractorId = model.ContractorId;
            edit.DivisionId = model.DivisionId;
            edit.DueDate = model.DueDate;
            edit.PaidOn = model.PaidDate;
            edit.Remarks = model.Remarks;
            edit.SubmittedToDivision = model.DivisionSubmittedDate;
            edit.SubmittedToFinance = model.FinanceSubmittedDate;
            _db.SubmitChanges();
        }
    }
}