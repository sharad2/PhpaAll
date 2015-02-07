using PhpaAll.Bills;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Linq;

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
                if (query == null) {
                    query = query1;
                }
                else
                {
                    query = query.Union(query1);
                }
            }

            // Max 200
            query = query.OrderByDescending(p => p.BillDate).Take(200);

            SearchViewModel model = new SearchViewModel
            {
                Bills = BillModel.FromQuery<SearchBillModel>(query).ToList()
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