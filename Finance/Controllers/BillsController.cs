using PhpaAll.Bills;
using PhpaAll.MvcHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhpaAll.Controllers
{
    /// <summary>
    /// This controller contains all readonly bill actions
    /// </summary>
    [Authorize(Roles = "Bills")]
    public partial class BillsController : Controller
    {

        private Lazy<PhpaBillsDataContext> _db;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            _db = new Lazy<PhpaBillsDataContext>(() => new PhpaBillsDataContext(requestContext.HttpContext));
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null && _db.IsValueCreated)
            {
                _db.Value.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Display recent bills. Option to create new bill
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "BillsExecutive")]
        public virtual ActionResult RecentBills(string[] approvers, int?[] divisions, int?[] processingDivisions, int?[] contractors, int?[] stations,
            DateTime? dateFrom, DateTime? dateTo, Decimal? minAmount, Decimal? maxAmount, bool? approved, bool exportToExcel = false)
        {

            var query = from bill in _db.Value.Bills
                        group bill by new
                        {
                            ApprovedBy = (bill.ApprovedBy ?? "").Trim() == "" ? "" : bill.ApprovedBy,
                            bill.Division,
                            bill.Contractor,
                            bill.Station,
                            bill.AtDivision
                        } into g
                        select new
                        {
                            ApprovedBy = g.Key.ApprovedBy,
                            SubmittedToDivisionId = g.Key.Division == null ? (int?)null : g.Key.Division.DivisionId,
                            ContractorId = g.Key.Contractor == null ? (int?)null : g.Key.Contractor.ContractorId,
                            DivisionName = g.Key.Division.DivisionName,
                            CurrentDivisionId = g.Key.AtDivision == null ? (int?)null : g.Key.AtDivision.DivisionId,
                            CurrentDivisionName = g.Key.AtDivision.DivisionName,
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
                ProcessingDivisions = (from d in aggQuery
                                       group d by d.CurrentDivisionId into g
                                       select new RecentBillsFilterModel
                                       {
                                           Id = string.Format("{0}", g.Key),
                                           Name = g.Select(p => p.CurrentDivisionName).FirstOrDefault(),
                                           Count = g.Sum(p => p.Count),
                                           Selected = processingDivisions == null || processingDivisions.Contains(g.Key)
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
                filteredBills = filteredBills.Where(p => divisions.Contains(p.DivisionId));
                model.IsFiltered = true;
            }

            if (processingDivisions != null && processingDivisions.Length > 0)
            {
                filteredBills = filteredBills.Where(p => processingDivisions.Contains(p.AtDivisionId));
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

            if (approved.HasValue)
            {
                if (approved.Value)
                {
                    filteredBills = filteredBills.Where(p => p.ApprovedOn != null);
                }
                else
                {
                    filteredBills = filteredBills.Where(p => p.ApprovedOn == null);
                }
                model.IsFiltered = true;
                model.FilterApprovedBills = approved;
            }

            if (false)
            {
                // Paid filter
                filteredBills = filteredBills.Where(p => p.Voucher != null);
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
            filteredBills = filteredBills.OrderByDescending(p => p.BillDate).Take(200);

            model.Bills = BillModel.FromQuery<BillModel>(filteredBills).ToList();

            foreach (var bill in model.Bills)
            {
                bill.CheckBoxName = MVC.Bills.Actions.ApproveBillsParams.listBillId;
            }

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
        [Authorize(Roles = "BillsManager")]
        [HttpPost]
        public virtual ActionResult ApproveBills(int[] listBillId, string[] approvers, int[] divisions, int[] processingDivisions, int[] contractors,
                                                int[] stations, DateTime? dateFrom, DateTime? dateTo, Decimal? minAmount, Decimal? maxAmount,
                                                bool approve, bool? approvedFilter)
        {
            if (string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                throw new HttpException("You must be logged in to approve or disapprove bills");
            }
            if (listBillId != null)
            {
                var query = from bill in _db.Value.Bills
                            where listBillId.Contains(bill.Id)
                            select bill;

                foreach (var bill in query)
                {
                    if (approve)
                    {
                        bill.ApprovedOn = DateTime.Now;
                        bill.ApprovedBy = User.Identity.Name;
                    }
                    else
                    {
                        bill.ApprovedOn = null;
                        bill.ApprovedBy = null;
                    }

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

            if (processingDivisions != null)
            {
                dict.Add(Actions.RecentBillsParams.processingDivisions, processingDivisions);
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
                dict.Add(Actions.RecentBillsParams.maxAmount, new [] { maxAmount.Value });
            }
            if (minAmount != null)
            {
                dict.Add(Actions.RecentBillsParams.minAmount, new [] { minAmount.Value });
            }

            if (dateFrom != null)
            {
                dict.Add(Actions.RecentBillsParams.dateFrom, new [] { dateFrom.Value });
            }

            if (dateTo != null)
            {
                dict.Add(Actions.RecentBillsParams.dateTo, new [] { dateTo.Value });
            }

            if (approvedFilter.HasValue)
            {
                dict.Add(Actions.RecentBillsParams.approved, new [] { approvedFilter.Value });
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
        /// Approves a single bill. Designed to be called via AJAX
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "BillsManager")]
        public virtual ActionResult ApproveBill(int billId)
        {
            if (string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                throw new HttpException("You must be logged in to approve or disapprove bills");
            }
            var query = (from bill in _db.Value.Bills
                         where bill.Id == billId
                         select bill).SingleOrDefault();

            query.ApprovedOn = DateTime.Now;
            query.ApprovedBy = User.Identity.Name;
            _db.Value.SubmitChanges();

            return Json("Done");
        }

        private class MyTestGroup
        {

            public string GroupValue { get; set; }

            public int? GroupId { get; set; }

            public string GroupDisplayName { get; set; }
        }
        /// <summary>
        /// Display outstanding bills.
        /// </summary>
        [Authorize(Roles = "BillsExecutive")]
        [HttpGet]
        public virtual ActionResult OutstandingBills(bool? overdueOnly, OrderByField field = OrderByField.Division)
        {
            var model = new OutstandingBillsViewModel
            {
                OrderByField = field,
                OverDueOnly = overdueOnly
            };

            var query = from bill in _db.Value.Bills
                        where bill.Voucher == null
                        select bill;
            if (overdueOnly == true)
            {
                query = query.Where(p => p.DueDate < DateTime.Today);
            }

            IQueryable<IGrouping<MyTestGroup, Bill>> groupedQuery;

            switch (field)
            {
                case OrderByField.Division:
                    groupedQuery = from item in query
                                   group item by new MyTestGroup
                                   {
                                       GroupId = item.DivisionId,
                                       GroupValue = item.Division.DivisionName,
                                       GroupDisplayName = "Division"
                                   } into g
                                   select g;
                    break;
                case OrderByField.Station:
                    groupedQuery = from item in query
                                   group item by new MyTestGroup
                                   {
                                       GroupId = item.StationId,
                                       GroupValue = item.Station.StationName,
                                       GroupDisplayName = "Station"
                                   } into g
                                   select g;
                    break;
                case OrderByField.Contractor:
                    groupedQuery = from item in query
                                   group item by new MyTestGroup
                                   {
                                       GroupId = item.ContractorId,
                                       GroupValue = item.Contractor.ContractorName,
                                       GroupDisplayName = "Contractor"
                                   } into g
                                   select g;

                    break;
                default:
                    throw new NotImplementedException();
            }

            var finalquery = from mygroup in groupedQuery
                             orderby mygroup.Key.GroupValue
                             select new OutstandingBillGroupModel
                             {
                                 DatabaseCount = mygroup.Count(),
                                 GroupTotal = mygroup.Sum(p => p.Amount),
                                 GroupValue = mygroup.Key.GroupValue,
                                 GroupDisplayName = mygroup.Key.GroupDisplayName,
                                 Bills = (from bill in mygroup
                                          orderby bill.DueDate descending
                                          select new OutstandingBillModel
                                          {
                                              BillId = bill.Id,
                                              BillNumber = bill.BillNumber,
                                              DivisionId = bill.DivisionId,
                                              DivisionName = bill.Division.DivisionName,
                                              ContractorId = bill.ContractorId,
                                              ContractorName = bill.Contractor.ContractorName,
                                              BillDate = bill.BillDate,
                                              DueDate = bill.DueDate,
                                              Amount = bill.Amount
                                          }).Take(2000).ToList()

                             };
            model.BillGroups = finalquery.ToList();

            return View(Views.OutstandingBills, model);
        }
    }

}