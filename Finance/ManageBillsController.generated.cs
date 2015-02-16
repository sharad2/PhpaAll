// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
#pragma warning disable 1591, 3008, 3009, 0108
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace PhpaAll.Controllers
{
    public partial class ManageBillsController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ManageBillsController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ManageBillsController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CreateBill()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CreateBill);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Edit()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateOrDelete()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateOrDelete);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ShowBill()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ShowBill);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Image()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Image);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UploadImage()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UploadImage);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DeleteImage()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteImage);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DeleteImageofBill()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteImageofBill);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ApproveBill()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ApproveBill);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UnApproveBill()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UnApproveBill);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetDivision()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetDivision);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetContractor()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetContractor);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ManageBillsController Actions { get { return MVC.ManageBills; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "ManageBills";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "ManageBills";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Create = "Create";
            public readonly string CreateBill = "CreateBill";
            public readonly string Edit = "Edit";
            public readonly string UpdateOrDelete = "UpdateOrDelete";
            public readonly string ShowBill = "ShowBill";
            public readonly string Image = "Image";
            public readonly string UploadImage = "UploadImage";
            public readonly string DeleteImage = "DeleteImage";
            public readonly string DeleteImageofBill = "DeleteImageofBill";
            public readonly string ApproveBill = "ApproveBill";
            public readonly string UnApproveBill = "UnApproveBill";
            public readonly string GetDivision = "GetDivision";
            public readonly string GetContractor = "GetContractor";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Create = "Create";
            public const string CreateBill = "CreateBill";
            public const string Edit = "Edit";
            public const string UpdateOrDelete = "UpdateOrDelete";
            public const string ShowBill = "ShowBill";
            public const string Image = "Image";
            public const string UploadImage = "UploadImage";
            public const string DeleteImage = "DeleteImage";
            public const string DeleteImageofBill = "DeleteImageofBill";
            public const string ApproveBill = "ApproveBill";
            public const string UnApproveBill = "UnApproveBill";
            public const string GetDivision = "GetDivision";
            public const string GetContractor = "GetContractor";
        }


        static readonly ActionParamsClass_CreateBill s_params_CreateBill = new ActionParamsClass_CreateBill();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CreateBill CreateBillParams { get { return s_params_CreateBill; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CreateBill
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_UpdateOrDelete s_params_UpdateOrDelete = new ActionParamsClass_UpdateOrDelete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateOrDelete UpdateOrDeleteParams { get { return s_params_UpdateOrDelete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateOrDelete
        {
            public readonly string model = "model";
            public readonly string delete = "delete";
        }
        static readonly ActionParamsClass_ShowBill s_params_ShowBill = new ActionParamsClass_ShowBill();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ShowBill ShowBillParams { get { return s_params_ShowBill; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ShowBill
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_Image s_params_Image = new ActionParamsClass_Image();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Image ImageParams { get { return s_params_Image; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Image
        {
            public readonly string id = "id";
            public readonly string index = "index";
        }
        static readonly ActionParamsClass_UploadImage s_params_UploadImage = new ActionParamsClass_UploadImage();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UploadImage UploadImageParams { get { return s_params_UploadImage; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UploadImage
        {
            public readonly string billId = "billId";
            public readonly string file = "file";
        }
        static readonly ActionParamsClass_DeleteImage s_params_DeleteImage = new ActionParamsClass_DeleteImage();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteImage DeleteImageParams { get { return s_params_DeleteImage; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteImage
        {
            public readonly string billImageId = "billImageId";
        }
        static readonly ActionParamsClass_DeleteImageofBill s_params_DeleteImageofBill = new ActionParamsClass_DeleteImageofBill();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteImageofBill DeleteImageofBillParams { get { return s_params_DeleteImageofBill; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteImageofBill
        {
            public readonly string billId = "billId";
            public readonly string index = "index";
        }
        static readonly ActionParamsClass_ApproveBill s_params_ApproveBill = new ActionParamsClass_ApproveBill();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ApproveBill ApproveBillParams { get { return s_params_ApproveBill; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ApproveBill
        {
            public readonly string billId = "billId";
        }
        static readonly ActionParamsClass_UnApproveBill s_params_UnApproveBill = new ActionParamsClass_UnApproveBill();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UnApproveBill UnApproveBillParams { get { return s_params_UnApproveBill; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UnApproveBill
        {
            public readonly string billId = "billId";
        }
        static readonly ActionParamsClass_GetDivision s_params_GetDivision = new ActionParamsClass_GetDivision();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetDivision GetDivisionParams { get { return s_params_GetDivision; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetDivision
        {
            public readonly string term = "term";
        }
        static readonly ActionParamsClass_GetContractor s_params_GetContractor = new ActionParamsClass_GetContractor();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetContractor GetContractorParams { get { return s_params_GetContractor; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetContractor
        {
            public readonly string term = "term";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Bill = "Bill";
                public readonly string Bill_css = "Bill.css";
                public readonly string Bill_js = "Bill.js";
                public readonly string Create = "Create";
                public readonly string Edit = "Edit";
            }
            public readonly string Bill = "~/Views/ManageBills/Bill.cshtml";
            public readonly string Bill_css = "~/Views/ManageBills/Bill.css.bundle";
            public readonly string Bill_js = "~/Views/ManageBills/Bill.js.bundle";
            public readonly string Create = "~/Views/ManageBills/Create.cshtml";
            public readonly string Edit = "~/Views/ManageBills/Edit.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ManageBillsController : PhpaAll.Controllers.ManageBillsController
    {
        public T4MVC_ManageBillsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Create()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            CreateOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void CreateBillOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, PhpaAll.Bills.CreateViewModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult CreateBill(PhpaAll.Bills.CreateViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CreateBill);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            CreateBillOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Edit(int id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            EditOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void UpdateOrDeleteOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, PhpaAll.Controllers.EditViewModel model, bool? delete);

        [NonAction]
        public override System.Web.Mvc.ActionResult UpdateOrDelete(PhpaAll.Controllers.EditViewModel model, bool? delete)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateOrDelete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "delete", delete);
            UpdateOrDeleteOverride(callInfo, model, delete);
            return callInfo;
        }

        [NonAction]
        partial void ShowBillOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id);

        [NonAction]
        public override System.Web.Mvc.ActionResult ShowBill(int id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ShowBill);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ShowBillOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void ImageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id, int index);

        [NonAction]
        public override System.Web.Mvc.ActionResult Image(int id, int index)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Image);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "index", index);
            ImageOverride(callInfo, id, index);
            return callInfo;
        }

        [NonAction]
        partial void UploadImageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int billId, System.Web.HttpPostedFileBase file);

        [NonAction]
        public override System.Web.Mvc.ActionResult UploadImage(int billId, System.Web.HttpPostedFileBase file)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UploadImage);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "billId", billId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "file", file);
            UploadImageOverride(callInfo, billId, file);
            return callInfo;
        }

        [NonAction]
        partial void DeleteImageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int billImageId);

        [NonAction]
        public override System.Web.Mvc.ActionResult DeleteImage(int billImageId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteImage);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "billImageId", billImageId);
            DeleteImageOverride(callInfo, billImageId);
            return callInfo;
        }

        [NonAction]
        partial void DeleteImageofBillOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int billId, int index);

        [NonAction]
        public override System.Web.Mvc.ActionResult DeleteImageofBill(int billId, int index)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteImageofBill);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "billId", billId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "index", index);
            DeleteImageofBillOverride(callInfo, billId, index);
            return callInfo;
        }

        [NonAction]
        partial void ApproveBillOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int billId);

        [NonAction]
        public override System.Web.Mvc.ActionResult ApproveBill(int billId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ApproveBill);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "billId", billId);
            ApproveBillOverride(callInfo, billId);
            return callInfo;
        }

        [NonAction]
        partial void UnApproveBillOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int billId);

        [NonAction]
        public override System.Web.Mvc.ActionResult UnApproveBill(int billId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UnApproveBill);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "billId", billId);
            UnApproveBillOverride(callInfo, billId);
            return callInfo;
        }

        [NonAction]
        partial void GetDivisionOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string term);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetDivision(string term)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetDivision);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "term", term);
            GetDivisionOverride(callInfo, term);
            return callInfo;
        }

        [NonAction]
        partial void GetContractorOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string term);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetContractor(string term)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetContractor);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "term", term);
            GetContractorOverride(callInfo, term);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108
