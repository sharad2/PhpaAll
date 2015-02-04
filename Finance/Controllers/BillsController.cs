using PhpaAll.Bills;
using PhpaAll.MvcHelpers;
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
        public virtual ActionResult RecentBills(string[] approvers, int?[] divisions, int?[] contractors, int?[] stations,
            DateTime?[] dates, Decimal?[] amounts, bool exportToExcel = false)
        {
            //if (dates != null)
            //{
            //    throw new NotImplementedException(string.Format("Dates {0}", dates[0]));
            //}
            var query = from bill in _db.Value.Bills
                        group bill by new
                        {
                            bill.ApprovedBy,
                            bill.SubmittedToDivision,
                            bill.Contractor,
                            bill.Station
                        } into g
                        select new
                        {
                            ApprovedBy = g.Key.ApprovedBy,
                            SubmittedToDivisionId = g.Key.SubmittedToDivision == null ? (int?)null : g.Key.SubmittedToDivision.DivisionId,
                            ContractorId = g.Key.Contractor == null ? (int?)null : g.Key.Contractor.ContractorId,
                            DivisionName = g.Key.SubmittedToDivision.DivisionName,
                            ContractorName = g.Key.Contractor.ContractorName,
                            Count = g.Count(),
                            StationId = g.Key.Station == null ? (int?)null : g.Key.Station.StationId,
                            StationName = g.Key.Station.StationName
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
                             }).ToList(),

                Stations = (from d in aggQuery
                            group d by d.StationId into g
                            select new RecentBillsFilterModel
                            {
                                Id = string.Format("{0}", g.Key),
                                Name = g.Select(p => p.StationName).FirstOrDefault(),
                                Count = g.Sum(p => p.Count),
                                Selected = stations == null || stations.Contains(g.Key)
                            }).ToList(),
                UrlExcel = Request.RawUrl
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

            if (stations != null && stations.Length > 0)
            {
                filteredBills = filteredBills.Where(p => stations.Contains(p.StationId));
                model.IsFiltered = true;
            }

            if (dates != null)
            {
                // Assume that there will always be two dates
                if (dates[0] != null)
                {
                    // From Date
                    filteredBills = filteredBills.Where(p => p.BillDate >= dates[0]);
                    model.IsFiltered = true;
                    model.DateFrom = dates[0];
                }
                if (dates[1] != null)
                {
                    // From Date
                    filteredBills = filteredBills.Where(p => p.BillDate <= dates[1]);
                    model.IsFiltered = true;
                    model.DateTo = dates[1];
                }

            }
            if (amounts != null)
            {
                // Assume that there will always min and max amount value
                if (amounts[0] != null)
                {
                    // Min Amount
                    filteredBills = filteredBills.Where(p => p.Amount >= amounts[0]);
                    model.IsFiltered = true;
                    model.MinAmount = amounts[0];
                }
                if (amounts[1] != null)
                {
                    // Max Amount
                    filteredBills = filteredBills.Where(p => p.Amount <= amounts[1]);
                    model.IsFiltered = true;
                    model.MaxAmount = amounts[1];
                }

            }

            if (model.UrlExcel.Contains("?"))
            {
                model.UrlExcel += "&";
            }
            else
            {
                model.UrlExcel += "?";
            }
            model.UrlExcel += Actions.RecentBillsParams.exportToExcel + "=true";

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
                               ApprovedDate = bill.ApprovedOn,
                               Remarks = bill.Remarks,
                               SubmittedOnDate = bill.SubmittedOnDate,
                               Id = bill.Id,
                               StationId = bill.StationId,
                               StationName = bill.Station.StationName
                           }).Take(200).ToList();

            if (exportToExcel)
            {
                var result = new ExcelResult("List of Bills");
                result.AddWorkSheet(model.Bills, "Bills", "My Heading");
                return result;
            }
            return View(Views.RecentBills, model);
        }




    }
}