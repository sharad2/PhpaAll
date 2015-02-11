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
            DateTime? dateFrom, DateTime? dateTo, Decimal? minAmount, Decimal? maxAmount, bool exportToExcel = false)
        {
            //if (dates != null)
            //{
            //    throw new NotImplementedException(string.Format("Dates {0}", dates[0]));
            //}
            var query = from bill in _db.Value.Bills
                        group bill by new
                        {
                            ApprovedBy = (bill.ApprovedBy ?? "").Trim() == "" ? "" : bill.ApprovedBy,
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
                filteredBills = filteredBills.Where(p => processingDivisions.Contains(p.CurrentDivisionId));
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
            filteredBills = filteredBills.OrderByDescending(p => p.BillDate).Take(200);

            model.Bills = BillModel.FromQuery<BillModel>(filteredBills).ToList();

            foreach (var bill in model.Bills)
            {
                bill.CheckBoxName = MVC.Bills.Actions.ApproveBillsParams.listBillId;
            }

            //model.Bills = (from bill in filteredBills
            //               orderby bill.BillDate descending
            //               select new BillModel
            //               {

            //               }).Take(200).ToList();

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
            bool approve)
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
                dict.Add(Actions.RecentBillsParams.maxAmount, new decimal[] { maxAmount.Value });
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
                    //query = query.OrderBy(p => p.SubmittedToDivision.DivisionName).ThenBy(p => p.Station.StationName).ThenBy(p => p.Contractor.ContractorName).ThenBy(p => p.DueDate);
                    groupedQuery = from item in query
                                   group item by new MyTestGroup
                                   {
                                       GroupId = item.DivisionId,
                                       GroupValue = item.SubmittedToDivision.DivisionName,
                                       GroupDisplayName = "Division"
                                   } into g
                                   select g;
                    break;
                case OrderByField.Station:
                    //query = query.OrderBy(p => p.Station.StationName).ThenBy(p => p.SubmittedToDivision.DivisionName).ThenBy(p => p.Contractor.ContractorName).ThenBy(p => p.DueDate);
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
                    //query = query.OrderBy(p => p.Contractor.ContractorName).ThenBy(p => p.SubmittedToDivision.DivisionName).ThenBy(p => p.Station.StationName).ThenBy(p => p.DueDate);
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


            //var finalquery = from bill in query
            //                 select new OutstandingBillModel
            //                 {
            //                     BillId = bill.Id,
            //                     BillNumber = bill.BillNumber,
            //                     SubmittedToDivisionId = bill.SubmitedToDivisionId,
            //                     SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
            //                     ContractorId = bill.ContractorId,
            //                     ContractorName = bill.Contractor.ContractorName,
            //                     BillDate = bill.BillDate,
            //                     DueDate = bill.DueDate,
            //                     Amount = bill.Amount
            //                 };

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
                                              SubmittedToDivisionId = bill.DivisionId,
                                              SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
                                              ContractorId = bill.ContractorId,
                                              ContractorName = bill.Contractor.ContractorName,
                                              BillDate = bill.BillDate,
                                              DueDate = bill.DueDate,
                                              Amount = bill.Amount
                                          }).Take(2000).ToList()

                             };

            //var finalquery2 = from bill2 in query
            //                  group bill2 by bill2.SubmittedToDivision into g
            //                  orderby g.Key.DivisionName
            //                  select new OutstandingBillGroupModel
            //                  {
            //                      DatabaseCount = g.Count(),
            //                           GroupTotal = g.Sum(p => p.Amount),
            //                           OrderByValue = g.Key.DivisionName,
            //                           Bills = (from bill in g
            //                                    orderby bill.DueDate descending
            //                                    select new OutstandingBillModel
            //                                    {
            //                                        BillId = bill.Id,
            //                                        BillNumber = bill.BillNumber,
            //                                        SubmittedToDivisionId = bill.SubmitedToDivisionId,
            //                                        SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
            //                                        ContractorId = bill.ContractorId,
            //                                        ContractorName = bill.Contractor.ContractorName,
            //                                        BillDate = bill.BillDate,
            //                                        DueDate = bill.DueDate,
            //                                        Amount = bill.Amount
            //                                    }).Take(2000).ToList()

            //                  };

            //model.BillGroups = finalquery2.ToList();
            model.BillGroups = finalquery.ToList();
            //foreach (var row in finalquery2)
            //{
            //    model.Bills2[row.Division] = row.Bills.ToList();
            //}

            //model.Bills = finalquery.Take(5000).ToList();

            //foreach (var billmodel in model.Bills)
            //{
            //    switch (field)
            //    {
            //        case OrderByField.Division:
            //            billmodel.OrderByValue = billmodel.SubmittedToDivisionName;
            //            break;
            //        case OrderByField.Station:
            //            billmodel.OrderByValue = billmodel.StationName;
            //            break;
            //        case OrderByField.Contractor:
            //            billmodel.OrderByValue = billmodel.ContractorName;
            //            break;
            //        default:
            //            throw new NotImplementedException();
            //    }
            //}

            //var query2 = (from item in model.Bills
            //             group item by item.OrderByValue into g
            //             select new
            //             {
            //                 OrderByValue = g.Key,
            //                 Amount = g.Sum(p => p.Amount)
            //             }).ToDictionary(p => p.OrderByValue??"", p => p.Amount);
            //foreach (var billmodel in model.Bills)
            //{
            //    billmodel.GroupTotal = query2[billmodel.OrderByValue??""];
            //}

            return View(Views.OutstandingBills, model);
        }
    }

}