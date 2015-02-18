using PhpaAll.Bills;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhpaAll.Controllers
{
    public abstract partial class PhpaBaseController : Controller
    {

        internal Lazy<PhpaBillsDataContext> _db;

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

        public List<string> StatusMessages
        {
            get
            {
                return TempData["StatusMessages"] as List<string>;
            }
            private set
            {
                TempData["StatusMessages"] = value;
            }
        }

        protected void AddStatusMessage(string msg)
        {
            if (StatusMessages == null)
            {
                StatusMessages = new List<string>();
            }
            StatusMessages.Add(msg);
        }


    }
}