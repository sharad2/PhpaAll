using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhpaAll.Controllers
{
    public partial class BillsController : Controller
    {
        // GET: Bills
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}