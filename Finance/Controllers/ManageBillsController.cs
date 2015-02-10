using PhpaAll.Bills;
//using PhpaBills.Database;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
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
            var list = from stations in _db.Value.Stations select stations;
            model.StationList = list.Select(p => new SelectListItem
            {
                Text = p.StationName,
                Value = p.StationId.ToString()
            });

            return View(Views.Create, model);
        }


        [HttpPost]
        public virtual ActionResult CreateBill(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var x = ModelState.SelectMany(p => p.Value.Errors).Select(p => p.ErrorMessage);
                throw new NotImplementedException(string.Join(";", x));
            }
            //byte[] imageData = null;

            //if (model.BillImage != null && model.BillImage.ContentLength > 0)
            //{
            //    // Image Upload using MVC   http://cpratt.co/file-uploads-in-asp-net-mvc-with-view-models/  
            //    var ms = new MemoryStream(model.BillImage.ContentLength);
            //    model.BillImage.InputStream.CopyTo(ms);
            //    imageData = ms.ToArray();
            //}
            var bill = new Bill
            {
                Amount = model.Amount,
                BillNumber = model.BillNumber,
                Particulars = model.Particulars,
                BillDate = model.BillDate,
                //BillImage = imageData,
                ContractorId = model.ContractorId,
                SubmitedToDivisionId = model.SubmittedToDivisionId,
                DueDate = model.DueDate,
                //PaidDate = model.PaidDate,
                Remarks = model.Remarks,
                SubmittedOnDate = model.SubmittedOnDate,
                Id = model.Id,
                StationId = model.StationId
            };
            _db.Value.Bills.InsertOnSubmit(bill);
            _db.Value.SubmitChanges();
            return RedirectToAction(MVC.ManageBills.ShowBill(bill.Id));

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
                             //PaidDate = bill.PaidDate,
                             StationId = bill.StationId,
                             Remarks = bill.Remarks,
                             SubmittedOnDate = bill.SubmittedOnDate,
                             SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
                             ContractorName = bill.Contractor.ContractorName
                         }).FirstOrDefault();
            var list = from stations in _db.Value.Stations select stations;
            model.StationList = list.Select(p => new SelectListItem
            {
                Text = p.StationName,
                Value = p.StationId.ToString()
            });

            return View(Views.Edit, model);
        }

        [HttpPost]
        public virtual ActionResult Edit(EditViewModel model)
        {
            try
            {
                var edit = (from b in _db.Value.Bills
                            where b.Id == model.Id
                            select b).SingleOrDefault();

                edit.Amount = model.Amount;
                edit.Particulars = model.Particulars;
                edit.BillNumber = model.BillNumber;
                edit.BillDate = model.BillDate;
                //edit.BillImage = imageData;
                edit.ContractorId = model.ContractorId;
                edit.SubmitedToDivisionId = model.SubmittedToDivisionId;
                edit.DueDate = model.DueDate;
                edit.StationId = model.StationId;
                edit.Remarks = model.Remarks;
                edit.SubmittedOnDate = model.SubmittedOnDate;
                _db.Value.SubmitChanges();
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction(MVC.ManageBills.ShowBill(model.Id));
        }



        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            Bill edit = _db.Value.Bills.Where(bill => bill.Id == id).SingleOrDefault();

            //delete images first before attaimpting to delete the bill
            var deleteImages = from image in _db.Value.BillImages
                                          where image.BillId == id
                                          select image;
            foreach (var images in deleteImages)
            {
                _db.Value.BillImages.DeleteOnSubmit(images);
                
            }

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
                             //PaidDate = bill.PaidDate,
                             Remarks = bill.Remarks,
                             SubmittedOnDate = bill.SubmittedOnDate,
                             isEditMode = true,
                             SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
                             ContractorName = bill.Contractor.ContractorName,
                             ApprovedDate = bill.ApprovedOn,
                             ApprovedBy = bill.ApprovedBy,
                             StationName = bill.Station.StationName,
                             CurrentDivision = bill.CurrentDivision.DivisionName,
                             AttachedImageCount = bill.BillImages.Count
                         }).FirstOrDefault();

            // Getting Bill history from Bill Audit.  
            model.BillHistory = (from ba in _db.Value.BillAudits
                                 join bad in _db.Value.BillAuditDetails
                                 on ba.Id equals bad.BillAuditId
                                 where ba.BillId == id
                                 select new BillAuditModel(ba)
                                 ).ToList();

            return View(Views.Bill, model);
        }

        #region Image
        public virtual ActionResult Image(int id, int index)
        {
            //var model = (from bill in _db.Value.Bills
            //             where bill.Id == id
            //             select new BillViewModel
            //             {
            //                 BillImage = bill.BillImage
            //             }).FirstOrDefault();

            ////MemoryStream target = new MemoryStream();
            ////model.File.InputStream.CopyTo(target);
            //byte[] data = model.BillImage.ToArray();
            //return File(data, "image/jpg");

            var image = (from bill in _db.Value.BillImages
                         where bill.BillId == id
                         select new
                         {
                             ImageContentType = bill.ImageContentType,
                             Data = bill.BillImageData
                         }).Skip(index).FirstOrDefault();
            if (image == null)
            {
                throw new NotImplementedException();
            }
            // TODO: Get mime type from db
            return File(image.Data.ToArray(), image.ImageContentType);

        }

        [HttpPost]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public virtual ActionResult UploadImage(int billId, HttpPostedFileBase file)
        {
            //throw new NotImplementedException("Sorry");

            var imagesUploadedCount = (from imageCount in _db.Value.BillImages
                                       where imageCount.BillId == billId
                                       select imageCount.id).Count();

            if (imagesUploadedCount == 10)
            {
                throw new NotImplementedException("You can't upload more than 10 images");

            }
            else
            {
                var input = new byte[file.ContentLength];
                file.InputStream.Read(input, 0, file.ContentLength);

                var bill = new BillImage
                {
                    BillImageData = input,
                    BillId = billId,
                    ImageContentType = file.ContentType
                };
                _db.Value.BillImages.InsertOnSubmit(bill);
                _db.Value.SubmitChanges();
                return Json(new
                {
                    DeleteUrl = Url.Action(Actions.DeleteImage(bill.id))
                });
            }
        }

        /// <summary>
        /// Deletes the passed image given the ID of Bill Image table
        /// </summary>
        /// <param name="billImageId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult DeleteImage(int billImageId)
        {
            var query = (from image in _db.Value.BillImages
                         where image.id == billImageId
                         select image).FirstOrDefault();
            _db.Value.BillImages.DeleteOnSubmit(query);
            _db.Value.SubmitChanges();
            return Json("Done");
        }

        /// <summary>
        /// Deletes the passed image given the ID of Bill Image table
        /// </summary>
        /// <param name="billImageId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult DeleteImageofBill(int billId, int index)
        {
            var query = (from image in _db.Value.BillImages
                         where image.BillId == billId
                         select image).Skip(index).FirstOrDefault();
            _db.Value.BillImages.DeleteOnSubmit(query);
            _db.Value.SubmitChanges();
            return RedirectToAction(MVC.ManageBills.ShowBill(billId));
        }

        #endregion

        #region Autocomplete
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
        #endregion

    }
}