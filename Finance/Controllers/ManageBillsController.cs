using PhpaAll.Bills;
//using PhpaBills.Database;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhpaAll.Controllers
{
    public partial class ManageBillsController : Controller
    {
        //[Obsolete]
        //private Lazy<ManageBillsService> _service;

        private Lazy<PhpaBillsDataContext> _db;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //_service = new Lazy<ManageBillsService>(() => new ManageBillsService("default", requestContext.HttpContext.Trace));
            _db = new Lazy<PhpaBillsDataContext>(() => new PhpaBillsDataContext(requestContext.HttpContext.Trace));
        }

        protected override void Dispose(bool disposing)
        {
            //if (_service != null && _service.IsValueCreated)
            //{
            //    _service.Value.Dispose();
            //}

            if (_db != null && _db.IsValueCreated)
            {
                _db.Value.Dispose();
            }
            base.Dispose(disposing);
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
                Id = model.Id,
                StationId = 1  //TODO
            };
            _db.Value.Bills.InsertOnSubmit(bill);
            _db.Value.SubmitChanges();
            return RedirectToAction(MVC.Bills.RecentBills());

        }



        // GET:Edit
        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            var model = (from bill in _db.Value.Bills
                         where bill.Id == id
                         select new EditViewModel
                         {
                             Id = bill.Id,
                             Amount = bill.Amount,
                             BillNumber = bill.BillNumber,
                             Particulars = bill.Particulars,
                             BillDate = bill.BillDate,
                             ContractorId = bill.ContractorId,
                             SubmittedToDivisionId = bill.SubmitedToDivisionId,
                             DueDate = bill.DueDate,
                             PaidDate = bill.PaidDate,
                             Remarks = bill.Remarks,
                             SubmittedOnDate = bill.SubmittedOnDate,
                             SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
                             ContractorName = bill.Contractor.ContractorName
                         }).FirstOrDefault();

            return View(Views.Edit, model);
        }

        [HttpPost]
        public virtual ActionResult Edit(EditViewModel model)
        {


            try
            {
                byte[] imageData = null;

                if (model.BillImage != null && model.BillImage.ContentLength > 0)
                {
                    // Image Upload using MVC   http://cpratt.co/file-uploads-in-asp-net-mvc-with-view-models/  
                    var ms = new MemoryStream(model.BillImage.ContentLength);
                    model.BillImage.InputStream.CopyTo(ms);
                    imageData = ms.ToArray();
                }

                var edit = (from b in _db.Value.Bills
                            where b.Id == model.Id
                            select b).SingleOrDefault();

                edit.Amount = model.Amount;
                edit.Particulars = model.Particulars;
                edit.BillNumber = model.BillNumber;
                edit.BillDate = model.BillDate;
                edit.BillImage = imageData;
                edit.ContractorId = model.ContractorId;
                edit.SubmitedToDivisionId = model.SubmittedToDivisionId;
                edit.DueDate = model.DueDate;
                edit.PaidDate = model.PaidDate;
                edit.Remarks = model.Remarks;
                edit.SubmittedOnDate = model.SubmittedOnDate;
                _db.Value.SubmitChanges();
                return RedirectToAction(MVC.Bills.RecentBills());


            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction(MVC.ManageBills.ShowBill());
        }



        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            Bill edit = _db.Value.Bills.Where(bill => bill.Id == id).SingleOrDefault();
            if (edit != null)
            {
                _db.Value.Bills.DeleteOnSubmit(edit);
                _db.Value.SubmitChanges();
            }

            return RedirectToAction(MVC.Bills.RecentBills());
        }



        public virtual ActionResult ShowBill(int id)
        {

            var model = (from bill in _db.Value.Bills
                         where bill.Id == id
                         select new BillViewModel
                         {
                             Id = bill.Id,
                             Amount = bill.Amount,
                             BillNumber = bill.BillNumber,
                             Particulars = bill.Particulars,
                             BillDate = bill.BillDate,
                             ContractorId = bill.ContractorId,
                             SubmittedToDivisionId = bill.SubmitedToDivisionId,
                             DueDate = bill.DueDate,
                             PaidDate = bill.PaidDate,
                             Remarks = bill.Remarks,
                             SubmittedOnDate = bill.SubmittedOnDate,
                             isEditMode = true,
                             SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
                             ContractorName = bill.Contractor.ContractorName,
                             ApprovedDate = bill.ApprovedOn,
                             ApprovedBy = bill.ApprovedBy,
                             StationName = bill.Station.StationName
                            
                         }).FirstOrDefault();

            //// Dummy Code: TODO: Put where clause.  
            model.BillHistory = (from ba in _db.Value.BillAudits
                                 where ba.BillId == id
                                 select new BillAuditViewModel
                                 {
                                     BillCreatedBy = ba.CreatedBy,
                                     DateCreated = ba.Created,
                                     BillNumberOld = ba.BillNumberOld,
                                     BillNumberNew =ba.BillNumberNew

                                 }).ToList();

            return View(Views.Bill, model);
        }


        /// <summary>
        /// Get matching divisions
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public virtual JsonResult GetDivision(string term)
        {

            var data = from e in _db.Value.Divisions
                       where e.DivisionName.Contains(term)
                       orderby e.DivisionName
                       select new
                       {
                           label = e.DivisionName,
                           value = e.DivisionId
                       };
            return Json(data, JsonRequestBehavior.AllowGet);
        }




        public virtual JsonResult GetContractor(string term)
        {

            var data = from e in _db.Value.Contractors
                       where e.ContractorName.Contains(term)
                       orderby e.ContractorName
                       select new
                       {
                           label = e.ContractorName,
                           value = e.ContractorId
                       };
            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }
}