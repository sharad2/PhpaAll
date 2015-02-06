using System.Web.Mvc;
using System.Web.Security;

namespace PhpaAll.Controllers
{
    public partial class BillsHomeController : Controller
    {
        // GET: BillsHome
        public virtual ActionResult Index()
        {
            return View(Views.Index);
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