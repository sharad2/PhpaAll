using PhpaAll.Bills;
//using PhpaBills.Database;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace PhpaAll.Controllers
{
    [Authorize(Roles = "BillsOperator")]
    public partial class ManageBillsController : Controller
    {
        //[Obsolete]
        //private Lazy<ManageBillsService> _service;

        private Lazy<PhpaBillsDataContext> _db;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //_service = new Lazy<ManageBillsService>(() => new ManageBillsService("default", requestContext.HttpContext.Trace));
            _db = new Lazy<PhpaBillsDataContext>(() => new PhpaBillsDataContext(requestContext.HttpContext));
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

        [HttpGet]
        public virtual ActionResult Create()
        {
            var model = new CreateViewModel();
           // var list = from stations in _db.Value.Stations select stations;
            model.StationList = _db.Value.Stations.Select(p => new SelectListItem
            {
                Text = p.StationName,
                Value = p.StationId.ToString()
            });

            return View(Views.Create, model);
        }


        [HttpPost]
        public virtual ActionResult Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.StationList = _db.Value.Stations.Select(p => new SelectListItem
                {
                    Text = p.StationName,
                    Value = p.StationId.ToString()
                });
                return View(Views.Create, model);
            }
       
            var bill = new Bill
            {
                Amount = model.Amount,
                BillNumber = model.BillNumber,
                Particulars = model.Particulars,
                BillDate = model.BillDate,
                ContractorId = model.ContractorId,
                DivisionId = model.DivisionId,
                AtDivisionId = model.DivisionId,
                DueDate = model.DueDate,
                //PaidDate = model.PaidDate,
                Remarks = model.Remarks,
                ReceivedDate = model.ReceivedDate,
                Id = model.Id,
                StationId = model.StationId
            };
            _db.Value.Bills.InsertOnSubmit(bill);
            _db.Value.SubmitChanges();

            HttpPostedFileBase file = Request.Files[0];
            if(file.ContentLength > 0)
            {
            var input = new byte[file.ContentLength];
            file.InputStream.Read(input, 0, file.ContentLength);

            var billImage = new BillImage
            {
                BillImageData = input,
                BillId = bill.Id,
                ImageContentType = file.ContentType
            };
            _db.Value.BillImages.InsertOnSubmit(billImage);
            _db.Value.SubmitChanges();
        }
            return RedirectToAction(MVC.ManageBills.Create());

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
                             DivisionId = bill.DivisionId,
                             AtDivisionId = bill.AtDivisionId,
                             AtDivisionName = bill.AtDivision.DivisionName,
                             DueDate = bill.DueDate,
                             //PaidDate = bill.PaidDate,
                             StationId = bill.StationId,
                             Remarks = bill.Remarks,
                             ReceivedDate = bill.ReceivedDate,
                             DivisionName = bill.Division.DivisionName,
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
        public virtual ActionResult UpdateOrDelete(EditViewModel model, bool? delete)
        {
            if (delete.HasValue && delete.Value)
            {
                return DeleteBill(model.Id);
            }
            var edit = (from b in _db.Value.Bills
                        where b.Id == model.Id
                        select b).SingleOrDefault();

            edit.Amount = model.Amount;
            edit.Particulars = model.Particulars;
            edit.BillNumber = model.BillNumber;
            edit.BillDate = model.BillDate;
            edit.AtDivisionId = model.AtDivisionId;
            edit.ContractorId = model.ContractorId;
            edit.DivisionId = model.DivisionId;
            edit.DueDate = model.DueDate;
            edit.StationId = model.StationId;
            edit.Remarks = model.Remarks;
            edit.ReceivedDate = model.ReceivedDate;
            _db.Value.SubmitChanges();

            return RedirectToAction(MVC.ManageBills.ShowBill(model.Id));
        }

        private ActionResult DeleteBill(int id)
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
                             BillId = bill.Id,
                             Amount = bill.Amount,
                             BillNumber = bill.BillNumber,
                             Particulars = bill.Particulars,
                             BillDate = bill.BillDate,
                             ContractorId = bill.ContractorId,
                             DivisionId = bill.DivisionId,
                             DueDate = bill.DueDate,
                             //PaidDate = bill.PaidDate,
                             Remarks = bill.Remarks,
                             ReceivedDate = bill.ReceivedDate,
                             isEditMode = true,
                             DivisionName = bill.Division.DivisionName,
                             ContractorName = bill.Contractor.ContractorName,
                             ApprovedDate = bill.ApprovedOn,
                             ApprovedBy = bill.ApprovedBy,
                             StationName = bill.Station.StationName,
                             AtDivision = bill.AtDivision.DivisionName,
                             AttachedImageCount = bill.BillImages.Count
                         }).FirstOrDefault();

            // Getting Bill history from Bill Audit.  
            model.BillHistory = (from ba in _db.Value.BillAudit2s
                                 where ba.BillId == id
                                 orderby ba.Created descending
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
                                       select imageCount.Id).Count();

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
                    DeleteUrl = Url.Action(Actions.DeleteImage(bill.Id))
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
                         where image.Id == billImageId
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

        /// <summary>
        /// Approves a single bill.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "BillsManager")]
        public virtual ActionResult ApproveBill(int billId)
        {
            if (string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                throw new HttpException("You must be logged in to approve or disapprove bills");
            }
            var query = (from bill in _db.Value.Bills
                         where bill.Id == billId
                         select bill).SingleOrDefault();

            query.ApprovedOn = DateTime.Now;
            query.ApprovedBy = User.Identity.Name;
            _db.Value.SubmitChanges();
            return RedirectToAction(MVC.ManageBills.ShowBill(billId));
        }

        /// <summary>
        /// Unpproves a single bill. 
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "BillsManager")]
        public virtual ActionResult UnApproveBill(int billId)
        {
            if (string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                throw new HttpException("You must be logged in to approve or disapprove bills");
            }
            var query = (from bill in _db.Value.Bills
                         where bill.Id == billId
                         select bill).SingleOrDefault();

            query.ApprovedOn = null;
            query.ApprovedBy = null;
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