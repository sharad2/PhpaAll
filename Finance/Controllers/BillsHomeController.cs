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

        public virtual ActionResult Search(string searchText)
        {

            var query =
                (from bill in _db.Value.Bills
                 where bill.BillNumber.ToLower().Contains(searchText.ToLower())
                 orderby bill.BillDate descending
                 select new SearchModel
                 {
                     BillNumber = bill.BillNumber,
                     BillDate = bill.BillDate,
                     BillId = bill.Id,
                 }).Take(50).ToList();

            SearchViewModel model = new SearchViewModel
            {
                Bills = query
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