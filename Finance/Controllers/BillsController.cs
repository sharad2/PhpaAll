using PhpaAll.Bills;
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
        public virtual ActionResult RecentBills(string[] approvers, int?[] divisions)
        {
            var query = from bill in _db.Value.Bills
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
                        };

            // By taking ToList(), we execute the query here. Later we manipulate in memory version of the data
            var aggQuery = query.ToList();

            var model = new RecentBillsViewModel
            {
                Divisions = (from d in aggQuery
                             group d by d.DivisionId into g
                             select new RecentBillsFilterModel
                             {
                                 Id = string.Format("{0}", g.Key),
                                 Name = g.Select(p => p.DivisionName).FirstOrDefault(),
                                 Count = g.Sum(p => p.Count),
                                 Selected = divisions == null || divisions.Contains(g.Key)
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
                                 Id = string.Format("{0}", g.Key),
                                 Name = g.Key,
                                 Count = g.Sum(p => p.Count),
                                 Selected = approvers == null || approvers.Contains(g.Key ?? "")
                             }).ToList()
            };

            IQueryable<Bill> filteredBills = _db.Value.Bills;

            if (approvers != null && approvers.Length > 0)
            {
                filteredBills = filteredBills.Where(p => approvers.Contains(p.ApprovedBy ?? ""));
                model.IsFiltered = true;
            }

            // Max 200 bills
            model.Bills = (from bill in filteredBills
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
                           }).Take(200).ToList();

            return View(Views.RecentBills, model);
        }




        /// <summary>
        /// Get matching divisions
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public virtual JsonResult GetDivision(string term)
        {
           
            var data = from e in _db.Value.Divisions
                       where e.DivisionName.Contains(term)
                       orderby e.DivisionName
                       select new { 
                       label = e.DivisionName,
                       value=e.DivisionId
                       };
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Get matching contractor
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public virtual JsonResult GetContractor(string term)
        {
           
            var data = from e in _db.Value.Contractors
                       where e.ContractorName.StartsWith(term) 
                       orderby e.ContractorName
                       select new
                       {
                           label = e.ContractorName,
                           value = e.ContractorId
                       };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}