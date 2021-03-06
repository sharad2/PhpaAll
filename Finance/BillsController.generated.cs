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
    public partial class BillsController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public BillsController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected BillsController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult RecentBills()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RecentBills);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ApproveBills()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ApproveBills);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ApproveBill()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ApproveBill);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult OutstandingBills()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.OutstandingBills);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public BillsController Actions { get { return MVC.Bills; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Bills";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Bills";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string RecentBills = "RecentBills";
            public readonly string ApproveBills = "ApproveBills";
            public readonly string ApproveBill = "ApproveBill";
            public readonly string OutstandingBills = "OutstandingBills";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string RecentBills = "RecentBills";
            public const string ApproveBills = "ApproveBills";
            public const string ApproveBill = "ApproveBill";
            public const string OutstandingBills = "OutstandingBills";
        }


        static readonly ActionParamsClass_RecentBills s_params_RecentBills = new ActionParamsClass_RecentBills();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RecentBills RecentBillsParams { get { return s_params_RecentBills; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RecentBills
        {
            public readonly string approvers = "approvers";
            public readonly string divisions = "divisions";
            public readonly string processingDivisions = "processingDivisions";
            public readonly string contractors = "contractors";
            public readonly string stations = "stations";
            public readonly string dateFrom = "dateFrom";
            public readonly string dateTo = "dateTo";
            public readonly string dueDateFrom = "dueDateFrom";
            public readonly string dueDateTo = "dueDateTo";
            public readonly string dueDateNull = "dueDateNull";
            public readonly string minAmount = "minAmount";
            public readonly string maxAmount = "maxAmount";
            public readonly string paid = "paid";
            public readonly string exportToExcel = "exportToExcel";
        }
        static readonly ActionParamsClass_ApproveBills s_params_ApproveBills = new ActionParamsClass_ApproveBills();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ApproveBills ApproveBillsParams { get { return s_params_ApproveBills; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ApproveBills
        {
            public readonly string listBillId = "listBillId";
            public readonly string approvers = "approvers";
            public readonly string divisions = "divisions";
            public readonly string processingDivisions = "processingDivisions";
            public readonly string contractors = "contractors";
            public readonly string stations = "stations";
            public readonly string dateFrom = "dateFrom";
            public readonly string dateTo = "dateTo";
            public readonly string dueDateFrom = "dueDateFrom";
            public readonly string dueDateTo = "dueDateTo";
            public readonly string dueDateNull = "dueDateNull";
            public readonly string minAmount = "minAmount";
            public readonly string maxAmount = "maxAmount";
            public readonly string approve = "approve";
            public readonly string paidFilter = "paidFilter";
        }
        static readonly ActionParamsClass_ApproveBill s_params_ApproveBill = new ActionParamsClass_ApproveBill();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ApproveBill ApproveBillParams { get { return s_params_ApproveBill; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ApproveBill
        {
            public readonly string billId = "billId";
        }
        static readonly ActionParamsClass_OutstandingBills s_params_OutstandingBills = new ActionParamsClass_OutstandingBills();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_OutstandingBills OutstandingBillsParams { get { return s_params_OutstandingBills; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_OutstandingBills
        {
            public readonly string overdueOnly = "overdueOnly";
            public readonly string field = "field";
            public readonly string exportToExcel = "exportToExcel";
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
                public readonly string _recentBillsFilterPartial = "_recentBillsFilterPartial";
                public readonly string OutstandingBills = "OutstandingBills";
                public readonly string RecentBills = "RecentBills";
                public readonly string RecentBills_js = "RecentBills.js";
            }
            public readonly string _recentBillsFilterPartial = "~/Views/Bills/_recentBillsFilterPartial.cshtml";
            public readonly string OutstandingBills = "~/Views/Bills/OutstandingBills.cshtml";
            public readonly string RecentBills = "~/Views/Bills/RecentBills.cshtml";
            public readonly string RecentBills_js = "~/Views/Bills/RecentBills.js.bundle";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_BillsController : PhpaAll.Controllers.BillsController
    {
        public T4MVC_BillsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void RecentBillsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string[] approvers, int?[] divisions, int?[] processingDivisions, int?[] contractors, int?[] stations, System.DateTime? dateFrom, System.DateTime? dateTo, System.DateTime? dueDateFrom, System.DateTime? dueDateTo, bool? dueDateNull, decimal? minAmount, decimal? maxAmount, bool? paid, bool exportToExcel);

        [NonAction]
        public override System.Web.Mvc.ActionResult RecentBills(string[] approvers, int?[] divisions, int?[] processingDivisions, int?[] contractors, int?[] stations, System.DateTime? dateFrom, System.DateTime? dateTo, System.DateTime? dueDateFrom, System.DateTime? dueDateTo, bool? dueDateNull, decimal? minAmount, decimal? maxAmount, bool? paid, bool exportToExcel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RecentBills);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "approvers", approvers);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "divisions", divisions);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "processingDivisions", processingDivisions);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "contractors", contractors);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "stations", stations);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dateFrom", dateFrom);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dateTo", dateTo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dueDateFrom", dueDateFrom);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dueDateTo", dueDateTo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dueDateNull", dueDateNull);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "minAmount", minAmount);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "maxAmount", maxAmount);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "paid", paid);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "exportToExcel", exportToExcel);
            RecentBillsOverride(callInfo, approvers, divisions, processingDivisions, contractors, stations, dateFrom, dateTo, dueDateFrom, dueDateTo, dueDateNull, minAmount, maxAmount, paid, exportToExcel);
            return callInfo;
        }

        [NonAction]
        partial void ApproveBillsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int[] listBillId, string[] approvers, int[] divisions, int[] processingDivisions, int[] contractors, int[] stations, System.DateTime? dateFrom, System.DateTime? dateTo, System.DateTime? dueDateFrom, System.DateTime? dueDateTo, bool? dueDateNull, decimal? minAmount, decimal? maxAmount, bool approve, bool? paidFilter);

        [NonAction]
        public override System.Web.Mvc.ActionResult ApproveBills(int[] listBillId, string[] approvers, int[] divisions, int[] processingDivisions, int[] contractors, int[] stations, System.DateTime? dateFrom, System.DateTime? dateTo, System.DateTime? dueDateFrom, System.DateTime? dueDateTo, bool? dueDateNull, decimal? minAmount, decimal? maxAmount, bool approve, bool? paidFilter)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ApproveBills);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "listBillId", listBillId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "approvers", approvers);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "divisions", divisions);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "processingDivisions", processingDivisions);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "contractors", contractors);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "stations", stations);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dateFrom", dateFrom);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dateTo", dateTo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dueDateFrom", dueDateFrom);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dueDateTo", dueDateTo);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dueDateNull", dueDateNull);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "minAmount", minAmount);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "maxAmount", maxAmount);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "approve", approve);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "paidFilter", paidFilter);
            ApproveBillsOverride(callInfo, listBillId, approvers, divisions, processingDivisions, contractors, stations, dateFrom, dateTo, dueDateFrom, dueDateTo, dueDateNull, minAmount, maxAmount, approve, paidFilter);
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
        partial void OutstandingBillsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, bool? overdueOnly, PhpaAll.Bills.OrderByField field, bool exportToExcel);

        [NonAction]
        public override System.Web.Mvc.ActionResult OutstandingBills(bool? overdueOnly, PhpaAll.Bills.OrderByField field, bool exportToExcel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.OutstandingBills);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "overdueOnly", overdueOnly);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "field", field);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "exportToExcel", exportToExcel);
            OutstandingBillsOverride(callInfo, overdueOnly, field, exportToExcel);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108
