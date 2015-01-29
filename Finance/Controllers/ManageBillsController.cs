//using PhpaBills.Database;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using PhpaAll.Bills;

namespace PhpaAll.Controllers
{
    public partial class ManageBillsController : Controller
    {

        private Lazy<ManageBillsService> _service;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            _service = new Lazy<ManageBillsService>(() => new ManageBillsService("default"));
        }
        /// <summary>
        /// Display recent bills. Option to create new bill
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var model = new IndexViewModel
            {
                Bills = (from bill in _service.Value.Bills
                         orderby bill.BillDate descending
                         select new BillModel(bill)).ToList()
            };
            return View(Views.Index, model);
        }

        public virtual ActionResult Create()
        {
            var model = new CreateViewModel();            
            return View(Views.Create, model);
        }


        [HttpPost]
        public virtual ActionResult CreateBill(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(Views.Create, model);
            }

            // Image Upload using MVC   http://cpratt.co/file-uploads-in-asp-net-mvc-with-view-models/
            var bill = new Bill
            {
                Amount = model.Amount,
                ApprovedBy = model.ApprovedBy,
                BillNumber = model.BillNumber,
                ApprovedOn = model.ApprovedDate,
                BillDate = model.BillDate,
                //BillImage = model.BillImage,
                BillType = model.BillType,
                ContractorId = model.ContractorId,
                DivisionId = model.DivisionId,
                DueDate = model.DueDate,
                PaidOn = model.PaidDate,
                Remarks = model.Remarks,
                SubmittedToDivision = model.DivisionSubmittedDate,
                SubmittedToFinance = model.FinanceSubmittedDate,
            };
            _service.Value.InsertBill(bill);
            return RedirectToAction(Actions.Index());
        }



        // GET:Edit
        public virtual ActionResult Edit(string id)
        {
            throw new NotImplementedException();
          // CreateViewModel model = _service.Value.GetBillNumber(id);
          //return View(model);
                       
   

          //  return View(Views.Create, new CreateViewModel()
          //  {

          //      BillNumber = id,
          //      BillType = 
          //  });  
          
        }
        //public ActionResult Edit(int id)
        //{
        //    IndexViewModel model = _service.Value.Bills.Where(x => x.Id == id).Select(x =>
        //                        new BillModel()
        //                        {
        //                         BillNumber = x.BillNumber,
        //                          BillType = x.BillType, 
        //                        });

        //    PreparePublisher(model);
        //    return View(model);
        //}      
     
        // POST:Edit
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Edit(string id, FormCollection collection)
        {
             try
            {
                // TODO: Add update logic here           
                return RedirectToAction("Index");
            }
            catch
            {
                return View(Views.Index);
            }
        }
    }
}