using PhpaAll.Bills;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace PhpaAll.Controllers
{
    /// <summary>
    /// This controller contains all readonly bill actions
    /// </summary>
    public partial class BillsController : Controller
    {

        private Lazy<ManageBillsService> _service;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            _service = new Lazy<ManageBillsService>(() => new ManageBillsService("default", requestContext.HttpContext.Trace));
        }

        protected override void Dispose(bool disposing)
        {
            if (_service != null && _service.IsValueCreated)
            {
                _service.Value.Dispose();
            }
            base.Dispose(disposing);
        }

        public virtual ActionResult Index()
        {
            return View(Views.Index);
        }

        /// <summary>
        /// Display recent bills. Option to create new bill
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult RecentBills()
        {
            var model = new RecentBillsViewModel
            {
                Bills = (from bill in _service.Value.Bills
                         orderby bill.BillDate descending
                         select new BillModel
                         {
                             Amount = bill.Amount,
                             Particulars = bill.Particulars,
                             BillNumber = bill.BillNumber,
                             BillDate = bill.BillDate,
                             //BillImage = model.BillImage,          
                             ContractorId = bill.ContractorId,
                             ContractorName = bill.Contractor.ContractorName,
                             SubmittedToDivisionId = bill.SubmitedToDivisionId,
                             SubmittedToDivisionName = bill.Division.DivisionName,
                             DueDate = bill.DueDate,
                             PaidDate = bill.PaidDate,
                             Remarks = bill.Remarks,
                             SubmittedOnDate = bill.SubmittedOnDate,
                             Id = bill.Id,
                         }).ToList()
            };
            return View(Views.RecentBills, model);
        }
    }
}