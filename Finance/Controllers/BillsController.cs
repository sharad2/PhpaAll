using System.Web.Mvc;

namespace PhpaAll.Controllers
{
    /// <summary>
    /// This controller contains all readonly bill actions
    /// </summary>
    public partial class BillsController : Controller
    {
        // GET: BillReports
        public virtual ActionResult Index()
        {
            return View(Views.Index);
        }
    }
}