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
        public virtual System.Web.Mvc.ActionResult Search()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Search);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult RecentBills()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RecentBills);
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
            public readonly string Index = "Index";
            public readonly string Search = "Search";
            public readonly string RecentBills = "RecentBills";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Search = "Search";
            public const string RecentBills = "RecentBills";
        }


        static readonly ActionParamsClass_Search s_params_Search = new ActionParamsClass_Search();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Search SearchParams { get { return s_params_Search; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Search
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_RecentBills s_params_RecentBills = new ActionParamsClass_RecentBills();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RecentBills RecentBillsParams { get { return s_params_RecentBills; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RecentBills
        {
            public readonly string approvers = "approvers";
            public readonly string divisions = "divisions";
            public readonly string contractors = "contractors";
            public readonly string stations = "stations";
            public readonly string dates = "dates";
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
                public readonly string Index = "Index";
                public readonly string RecentBills = "RecentBills";
                public readonly string Search = "Search";
            }
            public readonly string Index = "~/Views/Bills/Index.cshtml";
            public readonly string RecentBills = "~/Views/Bills/RecentBills.cshtml";
            public readonly string Search = "~/Views/Bills/Search.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_BillsController : PhpaAll.Controllers.BillsController
    {
        public T4MVC_BillsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SearchOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Search(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Search);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            SearchOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void RecentBillsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string[] approvers, int?[] divisions, int?[] contractors, int?[] stations, System.DateTime?[] dates, bool exportToExcel);

        [NonAction]
        public override System.Web.Mvc.ActionResult RecentBills(string[] approvers, int?[] divisions, int?[] contractors, int?[] stations, System.DateTime?[] dates, bool exportToExcel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RecentBills);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "approvers", approvers);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "divisions", divisions);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "contractors", contractors);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "stations", stations);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dates", dates);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "exportToExcel", exportToExcel);
            RecentBillsOverride(callInfo, approvers, divisions, contractors, stations, dates, exportToExcel);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108
