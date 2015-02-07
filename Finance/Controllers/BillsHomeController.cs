﻿using PhpaAll.Bills;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;

namespace PhpaAll.Controllers
{
    public partial class BillsHomeController : Controller
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
        // GET: BillsHome
        public virtual ActionResult Index()
        {
            return View(Views.Index);
        }

        /// <summary>
        /// searchText can contain spaces in which case each word will be individually searched for
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public virtual ActionResult Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new NotImplementedException();
            }
            var tokens = searchText.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
            {
                throw new NotImplementedException();
            }
            //var query = (from bill in _db.Value.Bills
            //     where  bill.BillNumber.ToLower().Contains(searchText.ToLower())
            //     orderby bill.BillDate descending
            //     select bill).Take(50);
            IQueryable<Bill> query = null;

            foreach (var token in tokens)
            {
                var query1 = from bill in _db.Value.Bills
                         where bill.BillNumber.ToLower().Contains(token)
                         select bill;
                // Extra point if bill number starts with a token
                var query2 = from bill in _db.Value.Bills
                             where bill.BillNumber.ToLower().StartsWith(token)
                             select bill;
                // Extra point if bill number ends with token
                var query3 = from bill in _db.Value.Bills
                             where bill.BillNumber.ToLower().EndsWith(token)
                             select bill;


                if (query == null) {
                    query = query1;
                }
                else
                {
                    query = query.Concat(query1);
                }
                query = query.Concat(query2).Concat(query3);
            }

            // Max 200
            //query = query.OrderByDescending(p => p.BillDate);
            // Count how many times the bill was selected. If a bill is selected more times, it is more relevant
            var query44 = from bill in query
                         group bill by bill into g
                         let billNumber = g.Key.BillNumber.ToLower()
                         let exactMatch = tokens.Contains(billNumber) ? 1 : 0
                          //let startsWith = g.First().BillNumber.ToLower()
                         orderby exactMatch descending, g.Count() descending
                         select g.Key;

            var list = new List<SearchBillModel>();

            foreach (var sbm in BillModel.FromQuery<SearchBillModel>(query44).Take(200))
            {
                list.Add(sbm);
            }

            SearchViewModel model = new SearchViewModel
            {
                Bills = list
            };

            return View(Views.Search, model);
        }

        public virtual ActionResult Logoff()
        {
            if (this.Session != null)
            {
                //Clean session if it exists
                Session.Abandon();
            }
            FormsAuthentication.SignOut();
            return RedirectToAction(Actions.Index());
        }
    }
}