//using PhpaBills.Database;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using PhpaAll.Bills;
using System.Data;
using System.IO;

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
                         select new BillModel(bill)).ToList()
            };
            return View(Views.RecentBills, model);
        }

        public virtual ActionResult Create()
        {
            var model = new CreateViewModel();
            return View(Views.Create, model);
        }

        /// <summary>
        /// This method update existing bill and create new bills
        /// based on the id(primary key corresponding to Bill Number)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult CreateUpdateBill(CreateViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(Views.Create, model);
            }
            byte[] imageData = null;
           
            if (model.BillImage != null && model.BillImage.ContentLength > 0)
            {
                // Image Upload using MVC   http://cpratt.co/file-uploads-in-asp-net-mvc-with-view-models/  
                var ms = new MemoryStream(model.BillImage.ContentLength);
                model.BillImage.InputStream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            var bill = new Bill
            {
                Amount = model.Amount,           
                BillNumber = model.BillNumber,
                Particulars = model.Particulars,
                BillDate = model.BillDate,
                BillImage = imageData,               
                ContractorId = model.ContractorId,
                SubmitedToDivisionId = model.SubmittedToDivisionId,
                DueDate = model.DueDate,
                PaidDate = model.PaidDate,
                Remarks = model.Remarks,
                SubmittedOnDate = model.SubmittedOnDate,               
                Id= model.Id
            };   
            if (model.isEditMode)
            {
                
                try
                {
                    if (ModelState.IsValid)
                    {
                        //update the existing bill same id
                        _service.Value.UpdateBill(bill);
                        return RedirectToAction(MVC.ManageBills.RecentBills());
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
                return RedirectToAction(MVC.ManageBills.RecentBills());

            }
            else
            {
               //if no bill is there than insert new bill
                _service.Value.InsertBill(bill);
                return RedirectToAction(MVC.ManageBills.RecentBills());
            }
        }



        // GET:Edit
        /// <summary>
        /// Get the form to update a bill
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Edit(int id)
        {

            var bill = _service.Value.GetBillNumber(id);
            var model = new CreateViewModel
            {
                Id = bill.Id,
                Amount = bill.Amount,
                BillNumber = bill.BillNumber, 
                Particulars= bill.Particulars,
                BillDate = bill.BillDate,             
                ContractorId = bill.ContractorId,
                SubmittedToDivisionId = bill.SubmitedToDivisionId,
                DueDate = bill.DueDate,
                PaidDate = bill.PaidDate,
                Remarks = bill.Remarks,
                SubmittedOnDate = bill.SubmittedOnDate,             
                isEditMode = true

            };
            return View(Views.Create, model);
        }

        public virtual ActionResult Delete(int id)
        {
            try
            {
                _service.Value.DeleteBill(id);
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable delete. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction(MVC.ManageBills.RecentBills());
        }



        public virtual ActionResult Bill(int id)
        {
            return View(Views.Bill);
        }
    }
}