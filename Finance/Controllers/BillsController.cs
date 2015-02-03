﻿using PhpaAll.Bills;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhpaAll.Controllers
{
    /// <summary>
    /// This controller contains all readonly bill actions
    /// </summary>
    public partial class BillsController : Controller
    {

        private Lazy<PhpaBillsDataContext> _db;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            _db = new Lazy<PhpaBillsDataContext>(() => new PhpaBillsDataContext(requestContext.HttpContext.Trace));
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null && _db.IsValueCreated)
            {
                _db.Value.Dispose();
            }
            base.Dispose(disposing);
        }

        public virtual ActionResult Index()
        {
            return View(Views.Index);
        }

        public virtual ActionResult Search(string id)
        {
            return View(Views.Search);
        }

        /// <summary>
        /// Display recent bills. Option to create new bill
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult RecentBills()
        {
            //var query = from bill in _db.Value.Bills
            //            group bill by 1 into g
            //            select new
            //            {
            //                x = g.Select(p => p.ApprovedBy).Distinct(),
            //                y = g.Select(p => p.ApprovedBy).Distinct()
            //            };

            var aggQuery = (from bill in _db.Value.Bills
                            group bill by new
                            {
                                bill.ApprovedBy,
                                bill.Division,
                                bill.Contractor
                            } into g
                            select new
                            {
                                g.Key.ApprovedBy,
                                DivisionId = g.Key.Division == null ? (int?)null : g.Key.Division.DivisionId,
                                ContractorId = g.Key.Contractor == null ? (int?)null : g.Key.Contractor.ContractorId,
                                g.Key.Division.DivisionName,
                                g.Key.Contractor.ContractorName,
                                Count = g.Count()
                            }).ToList();

            var model = new RecentBillsViewModel
            {
                Divisions = (from d in aggQuery
                             group d by d.DivisionId into g
                             select new RecentBillsFilterModel
                             {
                                 Id = string.Format("{0}", g.Key),
                                 Name = g.Select(p => p.DivisionName).FirstOrDefault(),
                                 Count = g.Sum(p => p.Count)
                             }).ToList(),
                Contractors = (from d in aggQuery
                               group d by d.ContractorId into g
                               select new RecentBillsFilterModel
                               {
                                   Id = string.Format("{0}", g.Key),
                                   Name = g.Select(p => p.ContractorName).FirstOrDefault(),
                                   Count = g.Sum(p => p.Count)
                               }).ToList(),
                Approvers = (from d in aggQuery
                             group d by d.ApprovedBy into g
                             select new RecentBillsFilterModel
                             {
                                 Id = g.Key,
                                 Name = g.Key,
                                 Count = g.Sum(p => p.Count)
                             }).ToList(),
                Bills = (from bill in _db.Value.Bills
                         orderby bill.BillDate descending
                         select new BillModel
                         {
                             Amount = bill.Amount,
                             Particulars = bill.Particulars,
                             BillNumber = bill.BillNumber,
                             BillDate = bill.BillDate,
                             //BillImage = model.BillImage,          
                             ContractorId = bill.ContractorId,
                             ContractorName = bill.Contractor.ContractorName,
                             SubmittedToDivisionId = bill.SubmitedToDivisionId,
                             SubmittedToDivisionName = bill.Division.DivisionName,
                             DueDate = bill.DueDate,
                             PaidDate = bill.PaidDate,
                             Remarks = bill.Remarks,
                             SubmittedOnDate = bill.SubmittedOnDate,
                             Id = bill.Id,
                         }).ToList()
            };
            return View(Views.RecentBills, model);
        }
    }
}