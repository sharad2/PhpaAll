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
        public virtual ActionResult RecentBills(string[] approvers, int?[] divisions, int?[] contractors)
        {
            var query = from bill in _db.Value.Bills
                        group bill by new
                        {
                            bill.ApprovedBy,
                            bill.SubmittedToDivision,
                            bill.Contractor
                        } into g
                        select new
                        {
                            g.Key.ApprovedBy,
                            SubmittedToDivisionId = g.Key.SubmittedToDivision == null ? (int?)null : g.Key.SubmittedToDivision.DivisionId,
                            ContractorId = g.Key.Contractor == null ? (int?)null : g.Key.Contractor.ContractorId,
                            g.Key.SubmittedToDivision.DivisionName,
                            g.Key.Contractor.ContractorName,
                            Count = g.Count()
                        };

            // By taking ToList(), we execute the query here. Later we manipulate in memory version of the data
            var aggQuery = query.ToList();

            var model = new RecentBillsViewModel
            {
                Divisions = (from d in aggQuery
                             group d by d.SubmittedToDivisionId into g
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
                                   Count = g.Sum(p => p.Count),
                                   Selected = contractors == null || contractors.Contains(g.Key)
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

            if (divisions != null && divisions.Length > 0)
            {
                filteredBills = filteredBills.Where(p => divisions.Contains(p.SubmitedToDivisionId));
                model.IsFiltered = true;
            }

            if (contractors != null && contractors.Length > 0)
            {
                filteredBills = filteredBills.Where(p => contractors.Contains(p.ContractorId));
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
                               SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
                               DueDate = bill.DueDate,
                               PaidDate = bill.PaidDate,
                               Remarks = bill.Remarks,
                               SubmittedOnDate = bill.SubmittedOnDate,
                               Id = bill.Id,
                               StationId = bill.StationId,
                               StationName = bill.Station.StationName
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
            // Change null to empty string
            term = term ?? string.Empty;

            var tokens = term.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToList();

            string searchId;
            string searchDescription;

            switch (tokens.Count)
            {
                case 0:
                    // All division
                    searchId = searchDescription = string.Empty;
                    break;

                case 1:
                    // Try to match term with either id or description
                    searchId = searchDescription = tokens[0];
                    break;

                case 2:
                    // Try to match first token with id and second with description
                    searchId = tokens[0];
                    searchDescription = tokens[1];
                    break;

                default:
                    // For now, ignore everything after the second :
                    searchId = tokens[0];
                    searchDescription = tokens[1];
                    break;


            }

            //var data = from division in _db.Value.Divisions.Select(p => new
            //{
            //    label = string.Format("{0}: {1}", p.DivisionId, p.DivisionName),
            //    value = p.DivisionId
            //})
            var data = "xyz:1234" ;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}