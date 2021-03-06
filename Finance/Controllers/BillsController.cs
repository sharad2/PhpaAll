﻿using Eclipse.PhpaLibrary.Web.Providers;
using PhpaAll.Bills;
using PhpaAll.MvcHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace PhpaAll.Controllers
{
    /// <summary>
    /// This controller contains all readonly bill actions
    /// </summary>
    [Authorize(Roles = "Bills")]
    public partial class BillsController : PhpaBaseController
    {

        private const string ROLE_APPROVE = "BillsManager";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="approvers">Pass " " to see unapproved bills. Pass "*" to see approved bills. Pass individual approver names to see bills approved by
        /// specific approvers. If "*" is passed, all other entries in the array are ignored</param>
        /// <param name="divisions"></param>
        /// <param name="processingDivisions"></param>
        /// <param name="contractors"></param>
        /// <param name="stations"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="dueDateFrom"></param>
        /// <param name="dueDateTo"></param>
        /// <param name="dueDateNull"></param>
        /// <param name="minAmount"></param>
        /// <param name="maxAmount"></param>
        /// <param name="approved"></param>
        /// <param name="paid"></param>
        /// <param name="exportToExcel"></param>
        /// <returns></returns>
        [Authorize(Roles = "BillsExecutive")]
        public virtual ActionResult RecentBills(string[] approvers, int?[] divisions, int?[] processingDivisions, int?[] contractors, int?[] stations,
            DateTime? dateFrom, DateTime? dateTo,
            DateTime? dueDateFrom, DateTime? dueDateTo, bool? dueDateNull,
            decimal? minAmount, decimal? maxAmount, bool? paid, bool exportToExcel = false)
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
                            DivisionId = g.Key.Division == null ? (int?)null : g.Key.Division.DivisionId,
                            ContractorId = g.Key.Contractor == null ? (int?)null : g.Key.Contractor.ContractorId,
                            DivisionName = g.Key.Division.DivisionName,
                            AtDivisionId = g.Key.AtDivision == null ? (int?)null : g.Key.AtDivision.DivisionId,
                            AtDivisionName = g.Key.AtDivision.DivisionName,
                            ContractorName = g.Key.Contractor.ContractorName,
                            //Count = g.Count(),
                            StationId = g.Key.Station == null ? (int?)null : g.Key.Station.StationId,
                            StationName = g.Key.Station.StationName
                        };

            // By taking ToList(), we execute the query here. Later we manipulate in memory version of the data
            var aggQuery = query.ToList();

            var model = new RecentBillsViewModel
            {
                Divisions = (from d in aggQuery
                             group d by d.DivisionId into g
                             select new RecentBillsFilterModel
                             {
                                 Id = g.Key.HasValue ? g.Key.Value.ToString() : " ",  // Need a space here to ensure that it gets posted
                                 Name = g.Select(p => p.DivisionName).FirstOrDefault(),
                                 //Count = g.Sum(p => p.Count),
                                 Selected = divisions == null || divisions.Contains(g.Key)
                             }).OrderBy(p => p.Name).ToList(),
                AtDivisions = (from d in aggQuery
                                       group d by d.AtDivisionId into g
                                       select new RecentBillsFilterModel
                                       {
                                           Id = g.Key.HasValue ? g.Key.Value.ToString() : " ",  // Need a space here to ensure that it gets posted
                                           Name = g.Select(p => p.AtDivisionName).FirstOrDefault(),
                                           //Count = g.Sum(p => p.Count),
                                           Selected = processingDivisions == null || processingDivisions.Contains(g.Key)
                                       }).OrderBy(p => p.Name).ToList(),
                Contractors = (from d in aggQuery
                               group d by d.ContractorId into g
                               select new RecentBillsFilterModel
                               {
                                   Id = g.Key.HasValue ? g.Key.Value.ToString() : " ",  // Need a space here to ensure that it gets posted
                                   Name = g.Select(p => p.ContractorName).FirstOrDefault(),
                                   //Count = g.Sum(p => p.Count),
                                   Selected = contractors == null || contractors.Contains(g.Key)
                               }).OrderBy(p => p.Name).ToList(),
                Approvers = (from d in aggQuery
                             group d by d.ApprovedBy into g
                             let user = string.IsNullOrWhiteSpace(g.Key) ? null : Membership.GetUser(g.Key) as PhpaMembershipUser
                             select new RecentBillsFilterModel
                             {
                                 Id = string.IsNullOrWhiteSpace(g.Key) ? " " : g.Key, // // Need a space here to ensure that it gets posted
                                 Name = string.IsNullOrWhiteSpace(g.Key) ? "(Unapproved)" : (user == null ? g.Key : user.FullName),
                                 //Count = g.Sum(p => p.Count),
                                 Selected = approvers == null || approvers.Any(p => string.Compare(p.Trim(), (g.Key ?? "").Trim(), true) == 0 ||
                                     (p == "*" && !string.IsNullOrWhiteSpace(g.Key)))
                             }).OrderBy(p => p.Name).ToList(),

                Stations = (from d in aggQuery
                            group d by d.StationId into g
                            select new RecentBillsFilterModel
                            {
                                Id = g.Key.HasValue ? g.Key.Value.ToString() : " ",  // Need a space here to ensure that it gets posted
                                Name = g.Select(p => p.StationName).FirstOrDefault(),
                                //Count = g.Sum(p => p.Count),
                                Selected = stations == null || stations.Contains(g.Key)
                            }).OrderBy(p => p.Name).ToList(),
                UrlExcel = Request.RawUrl,
                RoleApproveButtons = ROLE_APPROVE
            };

            IQueryable<Bill> filteredBills = _db.Value.Bills;

            if (approvers != null && approvers.Length > 0)
            {
                if (approvers.Contains("*"))
                {
                    // Special handling. Show all approved bills
                    filteredBills = filteredBills.Where(p => p.ApprovedBy != null);
                }
                else
                {
                    // Sanitize approvers. Important to change null to empty string to ensure that the where clause of the query succeeds
                    approvers = approvers.Select(p => (p ?? string.Empty).Trim().ToLower()).ToArray();
                    filteredBills = filteredBills.Where(p => approvers.Contains(p.ApprovedBy ?? ""));
                }
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
                var includeNulls = contractors.Any(p => p == null);
                filteredBills = filteredBills.Where(p => contractors.Contains(p.ContractorId) || (p.ContractorId == null && includeNulls));
                model.IsFiltered = true;
            }

            if (stations != null && stations.Length > 0)
            {
                filteredBills = filteredBills.Where(p => stations.Contains(p.StationId));
                model.IsFiltered = true;
            }


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

            if (dueDateNull.HasValue && dueDateNull.Value)
            {
                filteredBills = filteredBills.Where(p => p.DueDate == null);
                model.IsFiltered = true;
                model.FilterDueDateNull = true;
            }
            else
            {
                if (dueDateFrom != null)
                {
                    // From Date
                    filteredBills = filteredBills.Where(p => p.DueDate >= dueDateFrom);
                    model.IsFiltered = true;
                    model.DueDateFrom = dueDateFrom;
                }
                if (dueDateTo != null)
                {
                    // From Date
                    filteredBills = filteredBills.Where(p => p.DueDate <= dueDateTo);
                    model.IsFiltered = true;
                    model.DueDateTo = dueDateTo;
                }
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

            //if (approved.HasValue)
            //{
            //    if (approved.Value)
            //    {
            //        filteredBills = filteredBills.Where(p => p.ApprovedOn != null);
            //    }
            //    else
            //    {
            //        filteredBills = filteredBills.Where(p => p.ApprovedOn == null);
            //    }
            //    model.IsFiltered = true;
            //    model.FilterApprovedBills = approved;
            //}

            if (paid.HasValue)
            {
                // Paid filter
                if (paid.Value)
                {
                    filteredBills = filteredBills.Where(p => p.Voucher != null);
                }
                else
                {
                    filteredBills = filteredBills.Where(p => p.Voucher == null);
                }
                model.IsFiltered = true;
                model.FilterPaidBills = paid;
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

            // Populate the counts now
            var queryBillCounts = from item in model.Bills
                         group item by new
                         {
                             item.StationName,
                             item.DivisionName,
                             item.ContractorName,
                             item.ApprovedBy,
                             item.AtDivisionName
                         } into g
                         select new {
                             Group = g.Key,
                             Count = g.Count()
                         };

            foreach (var x in model.Contractors)
            {
                x.Count = queryBillCounts.Where(p => (p.Group.ContractorName ?? string.Empty) == (x.Name ?? string.Empty)).Sum(p => p.Count);
            }
            foreach (var x in model.Divisions)
            {
                x.Count = queryBillCounts.Where(p => p.Group.DivisionName == x.Name).Sum(p => p.Count);
            }
            foreach (var x in model.AtDivisions)
            {
                x.Count = queryBillCounts.Where(p => p.Group.AtDivisionName == x.Name).Sum(p => p.Count);
            }
            foreach (var x in model.Approvers)
            {
                //var z = queryBillCounts.Where(p => (p.Group.ApprovedBy ?? string.Empty) == (x.Name ?? string.Empty)).ToList();
                x.Count = queryBillCounts.Where(p => (p.Group.ApprovedBy ?? string.Empty).Trim() == (x.Id ?? string.Empty).Trim()).Sum(p => p.Count);
            }
            foreach (var x in model.Stations)
            {
                x.Count = queryBillCounts.Where(p => p.Group.StationName == x.Name).Sum(p => p.Count);
            }

            if (this.HttpContext.User.IsInRole(ROLE_APPROVE))
            {
                foreach (var bill in model.Bills)
                {
                    // Checkbox is needed only for unpaid bills
                    bill.CheckBoxName = bill.VoucherId == null ? MVC.Bills.Actions.ApproveBillsParams.listBillId : null;
                }
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
        [Authorize(Roles = ROLE_APPROVE)]
        [HttpPost]
        public virtual ActionResult ApproveBills(int[] listBillId, string[] approvers, int[] divisions, int[] processingDivisions, int[] contractors,
                                                int[] stations, DateTime? dateFrom, DateTime? dateTo,
            DateTime? dueDateFrom, DateTime? dueDateTo, bool? dueDateNull,
            decimal? minAmount, decimal? maxAmount,
                                                bool approve, bool? paidFilter)
        {
            if (string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                throw new HttpException("You must be logged in to approve or disapprove bills");
            }

            var countApproved = 0;
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
                    ++countApproved;
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
                dict.Add(Actions.RecentBillsParams.maxAmount, new[] { maxAmount.Value });
            }
            if (minAmount != null)
            {
                dict.Add(Actions.RecentBillsParams.minAmount, new[] { minAmount.Value });
            }

            if (dateFrom != null)
            {
                dict.Add(Actions.RecentBillsParams.dateFrom, new[] { dateFrom.Value.ToShortDateString() });
            }

            if (dateTo != null)
            {
                dict.Add(Actions.RecentBillsParams.dateTo, new[] { dateTo.Value.ToShortDateString() });
            }

            if (dueDateFrom != null)
            {
                dict.Add(Actions.RecentBillsParams.dueDateFrom, new[] { dueDateFrom.Value.ToShortDateString() });
            }

            if (dueDateTo != null)
            {
                dict.Add(Actions.RecentBillsParams.dueDateTo, new[] { dueDateTo.Value.ToShortDateString() });
            }

            if (dueDateNull.HasValue && dueDateNull.Value)
            {
                dict.Add(Actions.RecentBillsParams.dueDateNull, new[] { dueDateNull });
            }

            if (paidFilter.HasValue)
            {
                dict.Add(Actions.RecentBillsParams.paid, new[] { paidFilter.Value });
            }

            if (dict.Count > 0)
            {
                var query = from item in dict
                            from object val in item.Value
                            select string.Format("{0}={1}", item.Key, val);
                url += "?" + string.Join("&", query);
            }

            if (approve)
            {
                AddStatusMessage(string.Format("{0} bills approved", countApproved));
            }
            else
            {
                AddStatusMessage(string.Format("{0} bills unapproved", countApproved));
            }


            return Redirect(url);
        }

        /// <summary>
        /// Approves a single bill. Designed to be called via AJAX
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = ROLE_APPROVE)]
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
        public virtual ActionResult OutstandingBills(bool? overdueOnly, OrderByField field = OrderByField.Division, bool exportToExcel = false)
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
                                          orderby bill.DueDate
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
                                              Amount = bill.Amount,
                                              CurrentDivision = bill.AtDivision.DivisionName,
                                              ApprovedDate = bill.ApprovedOn,
                                              ApprovedBy = bill.ApprovedBy,
                                              CreatedDate = bill.Created,
                                              CreatedBy = bill.CreatedBy,
                                              StationName = bill.Station.StationName,
                                              ReceivedDate= bill.ReceivedDate,
                                              Particulars = bill.Particulars,
                                              Remarks = bill.Remarks
                                          }).Take(2000).ToList(),
                             };
            model.UrlExcel = Request.RawUrl;
            model.BillGroups = finalquery.ToList();
            if (model.UrlExcel.Contains("?"))
            {
                model.UrlExcel += "&";
            }
            else
            {
                model.UrlExcel += "?";
            }
            model.UrlExcel += Actions.OutstandingBillsParams.exportToExcel + "=true";


            if (exportToExcel)
            {
                var result = new ExcelResult("List of Outstanding Bills");
                result.AddWorkSheet(model.BillGroups.SelectMany(p => p.Bills).OrderBy(p => p.BillDate).ToList(), "Sharad", "Sgharad");

                return result;
            }
            return View(Views.OutstandingBills, model);
        }
    }

}