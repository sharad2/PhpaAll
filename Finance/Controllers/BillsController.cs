using PhpaAll.Bills;
using PhpaAll.MvcHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhpaAll.Controllers
{
    /// <summary>
    /// This controller contains all readonly bill actions
    /// </summary>
    [Authorize(Roles="Bills")]
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
       [Authorize(Roles = "BillsExecutive")]
         public virtual ActionResult RecentBills(string[] approvers, int?[] divisions, int?[] currentDivisions, int?[] contractors, int?[] stations,
            DateTime? dateFrom, DateTime? dateTo, Decimal? minAmount, Decimal? maxAmount, bool exportToExcel = false)
        {
            //if (dates != null)
            //{
            //    throw new NotImplementedException(string.Format("Dates {0}", dates[0]));
            //}
            var query = from bill in _db.Value.Bills
                        group bill by new
                        {
                            ApprovedBy = (bill.ApprovedBy ?? "").Trim() == ""  ? "" : bill.ApprovedBy,
                            bill.SubmittedToDivision,
                            bill.Contractor,
                            bill.Station,
                            bill.CurrentDivision
                        } into g
                        select new
                        {
                            ApprovedBy = g.Key.ApprovedBy,
                            SubmittedToDivisionId = g.Key.SubmittedToDivision == null ? (int?)null : g.Key.SubmittedToDivision.DivisionId,
                            ContractorId = g.Key.Contractor == null ? (int?)null : g.Key.Contractor.ContractorId,
                            DivisionName = g.Key.SubmittedToDivision.DivisionName,
                            CurrentDivisionId = g.Key.CurrentDivision == null ? (int?)null : g.Key.CurrentDivision.DivisionId,
                            CurrentDivisionName = g.Key.CurrentDivision.DivisionName,
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
                CurrentDivisions = (from d in aggQuery
                             group d by d.CurrentDivisionId into g
                             select new RecentBillsFilterModel
                             {
                                 Id = string.Format("{0}", g.Key),
                                 Name = g.Select(p => p.CurrentDivisionName).FirstOrDefault(),
                                 Count = g.Sum(p => p.Count),
                                 Selected = currentDivisions == null || currentDivisions.Contains(g.Key)
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
                                 Selected = approvers == null || approvers.Any(p => string.Compare(p.Trim(), g.Key, true) == 0)
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

            if (currentDivisions != null && currentDivisions.Length > 0)
            {
                filteredBills = filteredBills.Where(p => currentDivisions.Contains(p.CurrentDivisionId));
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

            // Assume that there will always be two dates
            if (dateFrom != null)
            {
                // From Date
                filteredBills = filteredBills.Where(p => p.BillDate >= dateFrom);
                model.IsFiltered = true;
                model.DateFrom = dateFrom;
            }
            if (dateTo != null)
            {
                // From Date
                filteredBills = filteredBills.Where(p => p.BillDate <= dateTo);
                model.IsFiltered = true;
                model.DateTo = dateTo;
            }

            // Assume that there will always min and max amount value
            if (minAmount != null)
            {
                // Min Amount
                filteredBills = filteredBills.Where(p => p.Amount >= minAmount);
                model.IsFiltered = true;
                model.FilterMinAmount = minAmount;
            }

            if (maxAmount != null)
            {
                // Max Amount
                filteredBills = filteredBills.Where(p => p.Amount <= maxAmount);
                model.IsFiltered = true;
                model.FilterMaxAmount = maxAmount;
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
                               ApprovedBy = bill.ApprovedBy,
                               Remarks = bill.Remarks,
                               SubmittedOnDate = bill.SubmittedOnDate,
                               Id = bill.Id,
                               StationId = bill.StationId,
                               StationName = bill.Station.StationName,
                               CurrentDivisionId = bill.CurrentDivisionId,
                               CurrentDivisionName = bill.CurrentDivision.DivisionName
                           }).Take(200).ToList();

            if (exportToExcel)
            {
                var result = new ExcelResult("List of Bills");
                result.AddWorkSheet(model.Bills, "Bills", "My Heading");
                return result;
            }
            return View(Views.RecentBills, model);
        }

        /// <summary>
        /// Only manager can approve the bills.
        /// </summary>
        /// <param name="listBillId"></param>
        /// <param name="approvalDate"></param>
        /// <param name="approvers">Used to pass to Recent Bills while redirecting</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "BillsManager")]
        [HttpPost]
        public virtual ActionResult ApproveBills(int[] listBillId, DateTime? approvalDate, string[] approvers, int[] divisions, int[] currentDivisions, int[] contractors, 
                                                int[] stations, DateTime? dateFrom, DateTime? dateTo, Decimal? minAmount, Decimal? maxAmount)
        {
            if (string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                throw new HttpException("You must be logged in to approve bills");
            }
            if (listBillId != null)
            {
                var query = from bill in _db.Value.Bills
                            where listBillId.Contains(bill.Id)
                            select bill;

                foreach (var bill in query)
                {
                    bill.ApprovedOn = approvalDate;
                    bill.ApprovedBy = User.Identity.Name;
                }
                _db.Value.SubmitChanges();
            }

            // Passing array as routevalues
            // http://stackoverflow.com/questions/8391055/passing-an-array-to-routevalues-and-have-it-render-model-binder-friendly-url
            var url = Url.Action(MVC.Bills.RecentBills());

            var dict = new Dictionary<string, IEnumerable>();

            if (approvers != null)
            {
                dict.Add(Actions.RecentBillsParams.approvers, approvers);
            }

            if (divisions != null)
            {
                dict.Add(Actions.RecentBillsParams.divisions, divisions);
            }

            if (currentDivisions != null)
            {
                dict.Add(Actions.RecentBillsParams.currentDivisions, currentDivisions);
            }

            if (contractors != null)
            {
                dict.Add(Actions.RecentBillsParams.contractors, contractors);
            }

            if (stations != null)
            {
                dict.Add(Actions.RecentBillsParams.stations, stations);
            }
            if (maxAmount != null)
            {
                dict.Add(Actions.RecentBillsParams.maxAmount, new decimal [] {maxAmount.Value});
            }
            if (minAmount != null)
            {
                dict.Add(Actions.RecentBillsParams.minAmount, new decimal[] { minAmount.Value });
            }

            if (dateFrom != null)
            {
                dict.Add(Actions.RecentBillsParams.dateFrom, new DateTime[] { dateFrom.Value });
            }

            if (dateTo != null)
            {
                dict.Add(Actions.RecentBillsParams.dateTo, new DateTime[] { dateTo.Value });
            }

            if (dict.Count > 0)
            {
                var query = from item in dict
                            from object val in item.Value
                            select string.Format("{0}={1}", item.Key, val);
                url += "?" + string.Join("&", query);
            }

            return Redirect(url);
        }

        /// <summary>
        /// Display outstanding bills.
        /// </summary>
          [Authorize(Roles = "BillsExecutive")]
        public virtual ActionResult OutstandingBills()
        {
            var model = new OutstandingBillsViewModel
            {

            };

            var query = from bill in _db.Value.Bills
                        where bill.PaidDate == null
                        orderby bill.DueDate descending
                        select new OutstandingBillModel
                        {
                            BillId = bill.Id,
                            BillNumber = bill.BillNumber,
                            SubmittedToDivisionId = bill.SubmitedToDivisionId,
                            SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
                            ContractorId = bill.ContractorId,
                            ContractorName = bill.Contractor.ContractorName,
                            BillDate = bill.BillDate,
                            DueDate = bill.DueDate,
                            Amount = bill.Amount
                        };

            model.Bills = query.Take(100).ToList();
            return View(Views.OutstandingBills, model);
        }
    }

}