using PhpaAll.Bills;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
                return RedirectToAction(MVC.Bills.RecentBills());
            }
            var queryFinal = SearchQuery(searchText);

            SearchViewModel model = new SearchViewModel
            {
                Bills = BillModel.FromQuery<SearchBillModel>(queryFinal).Take(200).ToList()
            };

            return View(Views.Search, model);
        }

        /// <summary>
        /// Splits the passed text into space seperated words. Looks for each word in multiple fields of bill.
        /// The bill which gets most hits is ranked at the top. Bill number hit is ranked better. bill number starts with or ends with hit is even better.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private IQueryable<Bill> SearchQuery(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentNullException("searchText");
            }
            var tokens = searchText.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
            {
                throw new NotImplementedException();
            }

            IQueryable<Bill> query = null;

            foreach (var token in tokens)
            {
                // It is a hit if any token is contained in any of the searrch fields
                var query1 = from bill in _db.Value.Bills
                             where bill.BillNumber.ToLower().Contains(token) ||
                                bill.Particulars.ToLower().Contains(token) ||
                                bill.Contractor.ContractorName.ToLower().Contains(token) ||
                                bill.SubmittedToDivision.DivisionName.ToLower().Contains(token) ||
                                bill.CurrentDivision.DivisionName.ToLower().Contains(token) ||
                                bill.Remarks.ToLower().Contains(token) ||
                                bill.Station.StationName.ToLower().Contains(token)
                             select bill;
                // Extra point if bill number starts with a token
                var query2 = from bill in _db.Value.Bills
                             where bill.BillNumber.ToLower().StartsWith(token)
                             select bill;
                // Extra point if bill number ends with token
                var query3 = from bill in _db.Value.Bills
                             where bill.BillNumber.ToLower().EndsWith(token)
                             select bill;


                if (query == null)
                {
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
            // Exact matches are best
            var queryFinal = from bill in query
                             group bill by bill into g
                             let billNumber = g.Key.BillNumber.ToLower()
                             let exactMatch = tokens.Contains(billNumber) ? 1 : 0
                             orderby exactMatch descending, g.Count() descending, g.Key.BillDate descending
                             select g.Key;

            return queryFinal;

        }

        private string GetAutocompleteText(Bill bill, string[] tokens)
        {
            string text1;
            string text2;
            if (bill.BillNumber != null && tokens.Any(p => bill.BillNumber.ToLower().Contains(p)))
            {
                // Bill number is always shown so we use particulars here
                text1 = string.Empty;
                text2 = bill.Particulars;
            }
            else if (bill.SubmittedToDivision != null && tokens.Any(p => bill.SubmittedToDivision.DivisionName.ToLower().Contains(p)))
            {
                // Bill number is always shown so we use particulars here
                text1 = "For Division";
                text2 = bill.SubmittedToDivision.DivisionName;
            }
            else if (bill.CurrentDivision != null && tokens.Any(p => bill.CurrentDivision.DivisionName.ToLower().Contains(p)))
            {
                // Bill number is always shown so we use particulars here
                text1 = "In Division";
                text2 = bill.CurrentDivision.DivisionName;
            }
            else if (bill.Contractor != null && tokens.Any(p => bill.Contractor.ContractorName.ToLower().Contains(p)))
            {
                // Bill number is always shown so we use particulars here
                text1 = "Contractor";
                text2 = bill.Contractor.ContractorName;
            }
            else if (bill.Station != null && tokens.Any(p => bill.Station.StationName.ToLower().Contains(p)))
            {
                // Bill number is always shown so we use particulars here
                text1 = "Station";
                text2 = bill.Station.StationName;
            }
            else
            {
                //throw new NotImplementedException();
                // Should never happen
                text1 = string.Empty;
                text2 = bill.Particulars;
            }


            return text1 + " " + HighlightTokens(text2, tokens);
        }

        private string HighlightTokens(string input, string[] tokens)
        {
            // Highlight each matching token in bill number and text
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            foreach (var token in tokens)
            {
                // Case insensitive string replace
                // http://stackoverflow.com/questions/6275980/string-replace-by-ignoring-case?lq=1
                input = Regex.Replace(input, token, "<strong>" + token + "</strong>", RegexOptions.IgnoreCase);
                //text2 = text2.Replace(token, "<strong>" + token + "</strong>");
            }

            return input;
        }

        public virtual ActionResult SearchAutoComplete(string searchText)
        {
            var query = SearchQuery(searchText).Take(50);

            var tokens = searchText.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var data = query.AsEnumerable().Select(bill => new
            {
                billId = bill.Id,
                particulars = HighlightTokens(bill.Particulars, tokens),
                billnumber = bill.BillNumber,
                date = string.Format("{0:d}", bill.BillDate),
                label = HighlightTokens(bill.BillNumber, tokens),
                text = GetAutocompleteText(bill, tokens)
            });

            return Json(data, JsonRequestBehavior.AllowGet);
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