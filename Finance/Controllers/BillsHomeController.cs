using PhpaAll.Bills;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace PhpaAll.Controllers
{
    public partial class BillsHomeController : Controller
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
        // GET: BillsHome
        public virtual ActionResult Index()
        {
            //SearchAutoComplete("a");
            return View(Views.Index);
        }

        /// <summary>
        /// searchText can contain spaces in which case each word will be individually searched for
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        /// <remarks>
        /// The passed search text is tokenized into words.
        /// For each word a query is constructed which looks for the word in ke fields (particulars, contractor name, etc).
        /// A union of all these queries is then executed.
        /// The resulting union query will return the bill multiple times if a bill matches multiple words.
        /// We count the number of times the bill is found and treat is as its relevance. The matched bills are displayed in the order of relevance.
        /// Only the top 200 results are displayed.
        /// </remarks>
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

        private class MyClass
        {

            public bool BillContains { get; set; }

            public bool BillStartsWith { get; set; }

            public Bill Bill { get; set; }

            public int Score { get; set; }
        };

        /// <summary>
        /// Splits the passed text into space seperated words. Looks for each word in multiple fields of bill.
        /// The bill which gets most hits is ranked at the top. Bill number hit is ranked better. bill number starts with or ends with hit is even better.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        /// <remarks>
        /// If no searchText is passed, the query simply returns all bills.
        /// </remarks>
        private IQueryable<Bill> SearchQuery(string searchText)
        {
            string[] tokens;
            IQueryable<MyClass> query;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                tokens = new string[0];
            }
            else
            {
                tokens = searchText.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (tokens.Length == 0)
            {
                // Search string is empty. Show all
                query = from bill in _db.Value.Bills
                        select new MyClass
                        {
                            Score = 100,
                            Bill = bill
                        };
            }
            else
            {
                query = null;
                foreach (var token in tokens)
                {
                    // If any token represnts an amount, search within 20% of bill amount
                    decimal amount;
                    bool isAmount = decimal.TryParse(token, out amount);

                    // If any token represents date, search for nearby bill dates
                    var date = ParseDate(token);
                    DateTime dateParam = date ?? DateTime.Today;

                    // It is a hit if any token is contained in any of the searrch fields
                    var query1 = from bill in _db.Value.Bills
                                 let dateMatch = (date.HasValue && bill.BillDate >= dateParam.AddDays(-7) && bill.BillDate <= dateParam.AddDays(7))
                                 let amountMatch = (isAmount && bill.Amount >= amount * 0.8m && bill.Amount <= 1.2m * amount)
                                 let billNumber = (bill.BillNumber ?? "").ToLower()
                                 let particularsMatch = bill.Particulars.ToLower().Contains(token)
                                 let contractorMatch = bill.Contractor.ContractorName.ToLower().Contains(token)
                                 let divisionMatch = bill.Division.DivisionName.ToLower().Contains(token)
                                 let currDivMatch = bill.AtDivision.DivisionName.ToLower().Contains(token)
                                 let remarksMatch = bill.Remarks.ToLower().Contains(token)
                                 let stationMatch = bill.Station.StationName.ToLower().Contains(token)
                                 where billNumber.Contains(token) || particularsMatch || contractorMatch ||
                                    divisionMatch || currDivMatch || remarksMatch || stationMatch ||
                                    amountMatch || dateMatch
                                 select new MyClass
                                 {
                                     Score = (billNumber.StartsWith(token) ? 100 : 0) +
                                         (billNumber.EndsWith(token) ? 100 : 0) +
                                         (billNumber.Contains(token) ? 100 : 0) +
                                         (billNumber == token ? 1000 : 0) +
                                         (particularsMatch ? 100 : 0) +
                                         (contractorMatch ? 100 : 0) +
                                         (divisionMatch ? 100 : 0) +
                                         (currDivMatch ? 100 : 0) +
                                         (remarksMatch ? 50 : 0) +
                                         (stationMatch ? 100 : 0) +
                                         (amountMatch ? 100 : 0) +
                                         (dateMatch ? 100 : 0),
                                     Bill = bill
                                 };


                    if (query == null)
                    {
                        query = query1;
                    }
                    else
                    {
                        query = query.Concat(query1);
                    }
                }
            }

            //query = query.OrderByDescending(p => p.BillDate);
            // Count how many times the bill was selected. If a bill is selected more times, it is more relevant
            // Exact matches are best
            var queryFinal = (from item in query
                              group item by item.Bill into g
                              orderby g.Sum(p => p.Score) descending, g.Key.BillDate descending
                              select g.Key);

            return queryFinal;

        }

        private DateTime? ParseDate(string token)
        {
            DateTime date;
            if (!DateTime.TryParse(token, out date))
            {
                return null;
            }
            if (date.Year <= 1900 || date.Year >= 9999)
            {
                // Date is outside SQL server limits
                // http://stackoverflow.com/questions/468045/error-sqldatetime-overflow-must-be-between-1-1-1753-120000-am-and-12-31-999
                return null;
            }
            // TODO: Make sure SQL server will love this date
            return date;
        }

        private string GetAutocompleteText(Bill bill, string[] tokens)
        {
            string text1;
            string text2;
            if (bill.BillNumber != null && tokens.Any(p => bill.BillNumber.ToLower().Contains(p)) ||
                bill.Particulars != null && tokens.Any(p => bill.Particulars.ToLower().Contains(p)))
            {
                // Bill number and particulars are always shown so no secondary text
                text1 = string.Empty;
                text2 = string.Empty;
            }
            else if (bill.Division != null && tokens.Any(p => bill.Division.DivisionName.ToLower().Contains(p)))
            {
                // Bill number is always shown so we use particulars here
                text1 = "For Division";
                text2 = bill.Division.DivisionName;
            }
            else if (bill.AtDivision != null && tokens.Any(p => bill.AtDivision.DivisionName.ToLower().Contains(p)))
            {
                // Bill number is always shown so we use particulars here
                text1 = "In Division";
                text2 = bill.AtDivision.DivisionName;
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
            else if (bill.Remarks != null && tokens.Any(p => bill.Remarks.ToLower().Contains(p)))
            {
                // Bill number is always shown so we use particulars here
                text1 = "";
                text2 = bill.Remarks;
            }
            else
            {
                text1 = string.Empty;
                text2 = string.Empty;
                // Should never happen
            }


            return text1 + " " + HighlightTokens(text2, tokens);
        }

        /// <summary>
        /// The function ensures that the hit is within first 64 characters. It truncates few leading characters if necessary
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
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
                input = Regex.Replace(input, token, "<span class='tt-highlight'>" + token + "</span>", RegexOptions.IgnoreCase);
                //text2 = text2.Replace(token, "<strong>" + token + "</strong>");
            }

            var index = input.IndexOf("<span");
            if (index > 64)
            {
                // Keep at most 32 characters before the hit
                var startAt = Math.Max(index - 32, 0);
                input = input.Substring(startAt);
                if (startAt > 0)
                {
                    // We have truncated something
                    input = "..." + input;
                }
            }

            return input;
        }

        /// <summary>
        /// If any of the tokens represents an amount, and the passed amount is within 20% of the token, highlight it
        /// </summary>
        /// <param name="billAmount"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private string HighlightAmount(decimal? billAmount, string[] tokens)
        {
            string fmtString = "{0:N0}";
            if (billAmount.HasValue)
            {
                decimal amount;
                foreach (var token in tokens)
                {
                    if (decimal.TryParse(token, out amount) && billAmount >= 0.8m * amount && billAmount <= 1.2m * amount)
                    {
                        fmtString = "<span class='tt-highlight'>" + fmtString + "</span>";
                        break;
                    }
                }
            }
            return string.Format(fmtString, billAmount);
        }

        private string HighlightDate(DateTime? billDate, string[] tokens)
        {
            string fmtString = "{0:d}";
            if (billDate.HasValue)
            {

                foreach (var token in tokens)
                {
                    var date = ParseDate(token);
                    if (date.HasValue && billDate >= date.Value.AddDays(-7) && billDate.Value <= date.Value.AddDays(7))
                    {
                        fmtString = "<span class='tt-highlight'>" + fmtString + "</span>";
                        break;
                    }
                }
            }
            return string.Format(fmtString, billDate);
        }

        /// <summary>
        /// Called by webform page insertvoucher.aspx. Returns only approved bills
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public virtual JsonResult BillsForDivisionAutoComplete(string term, int? divisionId)
        {
            var query = SearchQuery(term).Where(p => p.DivisionId == divisionId && p.ApprovedOn != null).Take(50);

            var tokens = term.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // AsEnumerable() executes the SQL query. After that we are free to use any C# function without SQL server complaining
            var data = query.AsEnumerable().Select(bill => new
            {
                Relevance = 100,
                Value = bill.Id,
                Text = string.Format("{0} <strong>{1:C}</strong><br/>{2}", bill.BillNumber, bill.Amount, bill.Particulars)
            }).ToList();

            return Json(new
            {
                d = data
            }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult SearchAutoComplete(string searchText)
        {
            var query = SearchQuery(searchText).Take(50);

            var tokens = searchText.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // AsEnumerable() executes the SQL query. After that we are free to use any C# function without SQL server complaining
            var data = query.AsEnumerable().Select(bill => new
            {
                // Used to redirect to the bill when something chosen from the list
                amount = HighlightAmount(bill.Amount, tokens),
                billId = bill.Id,
                // Always displayed in the list
                particulars = HighlightTokens(bill.Particulars, tokens),
                billNumber = HighlightTokens(bill.BillNumber, tokens),
                date = HighlightDate(bill.BillDate, tokens),
                // Highlighted text containing the hit
                text = GetAutocompleteText(bill, tokens)
            }).ToList();

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